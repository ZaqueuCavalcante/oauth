using Scalar.AspNetCore;

namespace OAuth.DrawApp.Middlewares;

public static class HttpMiddlewares
{
    public static void UseControllers(this IApplicationBuilder app)
    {
        app.UseEndpoints(options =>
        {
            options.MapControllers();

            options.MapOpenApi();

            options.MapScalarApiReference("/docs/v1", options =>
            {
                options.WithModels(false);
                options.WithDownloadButton(false);
                options.WithTitle("DrawApp API Docs");
                options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
            });
        });
    }
}
