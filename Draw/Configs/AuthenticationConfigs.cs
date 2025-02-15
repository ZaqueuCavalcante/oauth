using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OAuth.Draw.Features.CreateUser;
using OAuth.Draw.Features.CreateUserOAuthToken;

namespace OAuth.Draw.Configs;

public static class AuthenticationConfigs
{
    public const string DrawCookieScheme = "DrawCookie";
    public const string GoogleOAuthScheme = "GoogleOAuth";
    public const string GoogleOIDCScheme = "GoogleOIDC";

    public static void AddAuthenticationConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        var googleSettings = configuration.Google();

        services.AddAuthentication(DrawCookieScheme)
            .AddCookie(DrawCookieScheme, options =>
            {
                var baseHandler = options.Events.OnRedirectToAccessDenied;
                options.Events.OnRedirectToAccessDenied = ctx =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/google-drive"))
                    {
                        return ctx.HttpContext.ChallengeAsync(GoogleOAuthScheme);
                    }

                    return baseHandler(ctx);
                };
            })
            .AddOAuth(GoogleOAuthScheme, options =>
            {
                options.SignInScheme = DrawCookieScheme;

                options.ClientId = googleSettings.ClientId;
                options.ClientSecret = googleSettings.ClientSecret;
                options.AuthorizationEndpoint = googleSettings.AuthorizationEndpoint;
                options.TokenEndpoint = googleSettings.TokenEndpoint;
                options.CallbackPath = googleSettings.OAuthCallbackPath;

                options.SaveTokens = true;
                options.AdditionalAuthorizationParameters.Add("access_type", "offline");

                options.Scope.Clear();
                options.Scope.Add(googleSettings.DriveScope);

                options.Events.OnCreatingTicket = async ctx =>
                {
                    var dbCtx = ctx.HttpContext.RequestServices.GetRequiredService<DrawDbContext>();

                    var authHandlerProvider = ctx.HttpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                    var authHandler = await authHandlerProvider.GetHandlerAsync(ctx.HttpContext, DrawCookieScheme);
                    var authResult = await authHandler.AuthenticateAsync();

                    if (!authResult.Succeeded)
                    {
                        ctx.Fail($"Fail {DrawCookieScheme} authentication");
                        return;
                    }

                    var user = authResult.Principal;
                    var token = new UserOAuthToken(user.Id(), ctx.AccessToken, ctx.RefreshToken);

                    dbCtx.Add(token);
                    await dbCtx.SaveChangesAsync();

                    ctx.Principal = user.Clone();
                    var identity = ctx.Principal.Identities.First(x => x.AuthenticationType == DrawCookieScheme);
                    identity.AddClaim(new("drv", "true"));
                };
            })
            .AddOpenIdConnect(GoogleOIDCScheme, options =>
            {
                options.SignInScheme = DrawCookieScheme;

                options.ClientId = googleSettings.ClientId;
                options.ClientSecret = googleSettings.ClientSecret;
                options.CallbackPath = googleSettings.OIDCCallbackPath;
                options.Authority = googleSettings.OIDCAuthority;

                options.Scope.Add(googleSettings.EmailScope);

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                options.Events.OnUserInformationReceived = async ctx =>
                {
                    var dbCtx = ctx.HttpContext.RequestServices.GetRequiredService<DrawDbContext>();
                    var service = ctx.HttpContext.RequestServices.GetRequiredService<CreateUserService>();

                    var user = ctx.Principal;
                    var name = user.Claims.First(x => x.Type == "name").Value;
                    var email = user.Claims.First(x => x.Type == ClaimTypes.Email).Value;
                    var result = await service.Create(new(name, email, Guid.NewGuid().ToString()));

                    await dbCtx.SaveChangesAsync();
                };
            });
    }
}
