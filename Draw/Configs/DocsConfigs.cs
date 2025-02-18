using System.Reflection;
using Microsoft.OpenApi.Models;

namespace OAuth.Draw.Configs;

public static class DocsConfigs
{
    public static void AddDocsConfigs(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Draw API Docs",
                Description = ReadResource("api-intro.md"),
            });

            options.EnableAnnotations();

            options.TagActionsBy(api =>
            {
                return ["Endpoints"];
            });

            options.DocInclusionPredicate((name, api) => true);

            options.ExampleFilters();

            options.DescribeAllParametersInCamelCase();

            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Draw.xml");
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        });

        services.AddSwaggerExamplesFromAssemblyOf(typeof(Program));
    }

    private static string ReadResource(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name));

        using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
