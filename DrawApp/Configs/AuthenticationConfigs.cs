using System.Security.Claims;
using OAuth.DrawApp.Features.Login;
using OAuth.DrawApp.Features.CreateUser;
using Microsoft.AspNetCore.Authentication;
using OAuth.DrawApp.Features.CreateUserOAuthToken;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace OAuth.DrawApp.Configs;

public static class AuthenticationConfigs
{
    public const string DrawAppCookieScheme = "DrawAppCookie";
    public const string GoogleOAuthScheme = "GoogleOAuth";
    public const string GoogleOIDCScheme = "GoogleOIDC";

    public static void AddAuthenticationConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        var googleSettings = configuration.Google();

        services.AddAuthentication(DrawAppCookieScheme)
            .AddCookie(DrawAppCookieScheme, options =>
            {
                options.Events.OnRedirectToLogin = ctx =>
                {
                    ctx.Response.Clear();
                    ctx.Response.StatusCode = 401;
                    return Task.FromResult(0);
                };

                options.Events.OnRedirectToAccessDenied = ctx =>
                {
                    ctx.Response.Clear();
                    ctx.Response.StatusCode = 403;
                    return Task.FromResult(0);
                };
            })
            .AddOAuth(GoogleOAuthScheme, options =>
            {
                options.SignInScheme = DrawAppCookieScheme;

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
                    var dbCtx = ctx.HttpContext.RequestServices.GetRequiredService<DrawAppDbContext>();

                    var authHandlerProvider = ctx.HttpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                    var authHandler = await authHandlerProvider.GetHandlerAsync(ctx.HttpContext, DrawAppCookieScheme);
                    var authResult = await authHandler.AuthenticateAsync();

                    if (!authResult.Succeeded)
                    {
                        ctx.Fail($"Fail {DrawAppCookieScheme} authentication");
                        return;
                    }

                    var user = authResult.Principal;
                    var token = new UserOAuthToken(user.Id(), ctx.AccessToken, ctx.RefreshToken);

                    dbCtx.Add(token);
                    await dbCtx.SaveChangesAsync();

                    ctx.Principal = user.Clone();
                    var identity = ctx.Principal.Identities.First(x => x.AuthenticationType == DrawAppCookieScheme);
                    identity.AddClaim(new("drv", "true"));
                };
            })
            .AddOpenIdConnect(GoogleOIDCScheme, options =>
            {
                options.SignInScheme = DrawAppCookieScheme;

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
                    var dbCtx = ctx.HttpContext.RequestServices.GetRequiredService<DrawAppDbContext>();
                    var createUserService = ctx.HttpContext.RequestServices.GetRequiredService<CreateUserService>();
                    var loginService = ctx.HttpContext.RequestServices.GetRequiredService<LoginService>();

                    var user = ctx.Principal;
                    var name = user.Claims.First(x => x.Type == "name").Value;
                    var email = user.Claims.First(x => x.Type == ClaimTypes.Email).Value;
                    await createUserService.Create(new(name, email, Guid.NewGuid().ToString()));

                    var drawUser = await dbCtx.Users.FirstAsync(x => x.Email == email);
                    ctx.Principal = loginService.GetClaimsPrincipal(drawUser);
                };
            });
    }
}
