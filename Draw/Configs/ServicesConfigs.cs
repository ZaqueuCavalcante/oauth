namespace OAuth.Draw.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        services.AddServiceConfigs(typeof(IDrawService));
    }

    private static void AddServiceConfigs(this IServiceCollection services, Type marker)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName.StartsWith("Draw"))
            .SelectMany(s => s.GetTypes())
            .Where(p => marker.IsAssignableFrom(p) && !p.IsInterface)
            .ToList();

        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }
}
