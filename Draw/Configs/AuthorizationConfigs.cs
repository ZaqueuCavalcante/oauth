using OAuth.DrawApp.Auth;
using Microsoft.AspNetCore.Authorization;

namespace OAuth.DrawApp.Configs;

public static class AuthorizationConfigs
{
    public static void AddAuthorizationConfigs(this IServiceCollection services)
    {
        services.AddAuthorization(x => x.AddPolicies());
    }

    public static void AddPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(Policies.GoogleDriveEnabled, p => p
            .AddAuthenticationSchemes(AuthenticationConfigs.DrawAppCookieScheme)
            .RequireAuthenticatedUser()
            .RequireClaim("drv", "true"));
    }
}
