using OAuth.DrawApp.Security;

namespace OAuth.DrawApp.Configs;

public static class SecurityConfigs
{
    public static void AddSecurityConfigs(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
}
