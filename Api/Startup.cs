using OAuth.Api.Configs;
using OAuth.Api.Middlewares;

namespace OAuth.Api;

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

    public static void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();
        app.UseControllers();

        app.UseSwagger();
    }
}
