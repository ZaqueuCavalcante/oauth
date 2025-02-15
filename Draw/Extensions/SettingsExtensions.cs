using OAuth.Draw.Settings;

namespace OAuth.Draw.Extensions;

public static class SettingsExtensions
{
    public static GoogleSettings Google(this IConfiguration configuration) => new(configuration);
    public static DatabaseSettings Database(this IConfiguration configuration) => new(configuration);
}
