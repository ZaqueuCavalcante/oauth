namespace OAuth.Draw.Settings;

public class GoogleSettings
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string AuthorizationEndpoint { get; set; }
    public string TokenEndpoint { get; set; }
    public string OAuthCallbackPath { get; set; }
    public string DriveScope { get; set; }
    public string OIDCCallbackPath { get; set; }
    public string OIDCAuthority { get; set; }
    public string EmailScope { get; set; }

    public GoogleSettings(IConfiguration configuration)
    {
        configuration.GetSection("Google").Bind(this);
    }
}
