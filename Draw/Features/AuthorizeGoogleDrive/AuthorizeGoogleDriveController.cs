using OAuth.Draw.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace OAuth.Draw.Features.AuthorizeGoogleDrive;

[Authorize]
[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class AuthorizeGoogleDriveController : ControllerBase
{
    /// <summary>
    /// Redireciona pro Google, onde o usuário pode permitir o acesso do Draw ao seu Drive
    /// </summary>
    [HttpGet("oauth/google-drive")]
    public async Task Authorize()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = "/"
        };

        await HttpContext.ChallengeAsync(AuthenticationConfigs.GoogleOAuthScheme, properties);
    }
}
