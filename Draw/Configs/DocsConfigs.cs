using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Interfaces;

namespace OAuth.Draw.Configs;

public static class DocsConfigs
{
    public static void AddDocsConfigs(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "OAuth API Docs",
                Description = ReadResource("api-intro.md"),
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    { "x-logo", new OpenApiObject
                    {
                        { "url", new OpenApiString("/oauth-logo.png") },
                    }}
                },
            });

            options.EnableAnnotations();

            options.DocInclusionPredicate((name, api) => true);

            options.ExampleFilters();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Please enter a valid token",
            });

            options.DescribeAllParametersInCamelCase();

            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Api.xml");
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
