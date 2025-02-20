using OAuth.DrawApp.Settings;

namespace OAuth.DrawApp.Extensions;

public static class SettingsExtensions
{
    public static GoogleSettings Google(this IConfiguration configuration) => new(configuration);
    public static DatabaseSettings Database(this IConfiguration configuration) => new(configuration);
}
