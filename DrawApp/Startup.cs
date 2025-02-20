using OAuth.DrawApp.Configs;
using OAuth.DrawApp.Middlewares;

namespace OAuth.DrawApp;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServicesConfigs();

        services.AddIdentityConfigs();
        services.AddSecurityConfigs();

        services.AddAuthenticationConfigs(configuration);
        services.AddAuthorizationConfigs();

        services.AddHttpConfigs();
        services.AddEfCoreConfigs();

        services.AddOpenApi();
        services.AddDocsConfigs();
    }

    public static void Configure(IApplicationBuilder app, DrawAppDbContext ctx)
    {
        ctx.ResetDb();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();
        app.UseControllers();

        app.UseSwagger();
    }
}
