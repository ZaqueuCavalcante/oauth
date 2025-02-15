namespace OAuth.Draw.Features.OAuthCallback;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class OAuthCallbackController(OAuthCallbackService service) : ControllerBase
{
    /// <summary>
    /// 🔓 OAuth Callback
    /// </summary>
    [HttpGet("oauth/draw-callback")]
    public async Task<IActionResult> Callback()
    {
        var result = await service.Callback(new());

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
