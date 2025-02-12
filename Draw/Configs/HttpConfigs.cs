using System.Text.Json.Serialization;

namespace OAuth.Draw.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );

        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddHttpClient();
    }
}
