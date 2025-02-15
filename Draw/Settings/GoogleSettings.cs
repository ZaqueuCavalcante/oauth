namespace OAuth.Draw.Settings;

public class GoogleSettings
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string AuthorizationEndpoint { get; set; }
    public string TokenEndpoint { get; set; }
    public string CallbackPath { get; set; }
    public string DriveScope { get; set; }
    public string GrantType { get; set; }

    public GoogleSettings(IConfiguration configuration)
    {
        configuration.GetSection("Google").Bind(this);
    }
}
