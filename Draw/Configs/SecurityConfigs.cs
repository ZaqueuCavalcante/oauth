using OAuth.Draw.Security;

namespace OAuth.Draw.Configs;

public static class SecurityConfigs
{
    public static void AddSecurityConfigs(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
}
