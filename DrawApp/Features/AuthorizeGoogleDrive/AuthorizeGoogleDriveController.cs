using OAuth.DrawApp.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace OAuth.DrawApp.Features.LoginWithGoogle;

[Authorize]
[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class LoginWithGoogleController : ControllerBase
{
    /// <summary>
    /// Autorizar Google Drive
    /// </summary>
    [HttpGet("oauth/google-drive")]
    public async Task Authorize()
    {
        var properties = new AuthenticationProperties { RedirectUri = "/" };
        await HttpContext.ChallengeAsync(AuthenticationConfigs.GoogleOAuthScheme, properties);
    }
}
