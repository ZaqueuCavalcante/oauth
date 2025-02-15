namespace OAuth.Draw.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; set; }

    public DatabaseSettings(IConfiguration configuration)
    {
        configuration.GetSection("Database").Bind(this);
    }
}
