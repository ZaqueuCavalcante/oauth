using OAuth.Draw.Auth;
using Microsoft.AspNetCore.Authorization;

namespace OAuth.Draw.Features.CreateGoogleDriveFile;

[ApiController]
[Authorize(Policies.GoogleDriveEnabled)]
[Consumes("application/json"), Produces("application/json")]
public class CreateGoogleDriveFileController(CreateGoogleDriveFileService service) : ControllerBase
{
    /// <summary>
    /// Cria um arquivo .txt no Google Drive
    /// </summary>
    [HttpPost("goole-drive/files")]
    public async Task<IActionResult> Create([FromBody] CreateGoogleDriveFileIn data)
    {
        var result = await service.Create(User.Id(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
