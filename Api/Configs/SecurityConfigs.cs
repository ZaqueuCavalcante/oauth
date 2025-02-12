using OAuth.Api.Security;

namespace OAuth.Api.Configs;

public static class SecurityConfigs
{
    public static void AddSecurityConfigs(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
}
