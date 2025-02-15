using OAuth.Draw.Auth;
using Microsoft.AspNetCore.Authorization;

namespace OAuth.Draw.Configs;

public static class AuthorizationConfigs
{
    public static void AddAuthorizationConfigs(this IServiceCollection services)
    {
        services.AddAuthorization(x => x.AddPolicies());
    }

    public static void AddPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(Policies.GoogleDriveEnabled, p => p
            .AddAuthenticationSchemes(AuthenticationConfigs.DrawCookieScheme)
            .RequireAuthenticatedUser()
            .RequireClaim("drv", "true"));
    }
}
