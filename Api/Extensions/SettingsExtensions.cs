using OAuth.Api.Settings;

namespace OAuth.Api.Extensions;

public static class SettingsExtensions
{
    public static AuthSettings Auth(this IConfiguration configuration) => new(configuration);
    public static DatabaseSettings Database(this IConfiguration configuration) => new(configuration);
}
