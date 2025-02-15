using Microsoft.AspNetCore.Authentication;
using OAuth.Draw.Features.CreateUserOAuthToken;

namespace OAuth.Draw.Configs;

public static class AuthenticationConfigs
{
    public const string DrawCookieScheme = "DrawCookie";
    public const string GoogleOAuthScheme = "GoogleOAuth";

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
                options.CallbackPath = googleSettings.CallbackPath;

                options.SaveTokens = true;
                options.AdditionalAuthorizationParameters.Add("access_type", "offline");

                options.Scope.Clear();
                options.Scope.Add(googleSettings.DriveScope);

                // options.UsePkce = true;

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
            });
    }
}
