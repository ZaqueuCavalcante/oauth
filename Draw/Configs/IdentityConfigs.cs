using Microsoft.AspNetCore.Identity;

namespace OAuth.DrawApp.Configs;

public static class IdentityConfigs
{
    public static void AddIdentityConfigs(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>();
    }
}
