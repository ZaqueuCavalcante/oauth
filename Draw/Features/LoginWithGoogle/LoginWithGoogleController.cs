using OAuth.DrawApp.Configs;
using Microsoft.AspNetCore.Authentication;

namespace OAuth.DrawApp.Features.AuthorizeGoogleDrive;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class AuthorizeGoogleDriveController : ControllerBase
{
    /// <summary>
    /// Login com Google
    /// </summary>
    [HttpGet("login/google")]
    public async Task LoginWithGoogle()
    {
        var properties = new AuthenticationProperties { RedirectUri = "/" };
        await HttpContext.ChallengeAsync(AuthenticationConfigs.GoogleOIDCScheme, properties);
    }
}
