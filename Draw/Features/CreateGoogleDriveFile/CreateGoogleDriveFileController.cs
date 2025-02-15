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
    [HttpPost("google-drive/files")]
    public async Task<IActionResult> Create([FromBody] CreateGoogleDriveFileIn data)
    {
        var result = await service.Create(User.Id(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IMultipleExamplesProvider<CreateGoogleDriveFileIn>
{
    public IEnumerable<SwaggerExample<CreateGoogleDriveFileIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Única Linha",
			new CreateGoogleDriveFileIn {  Name = "OneLine.txt", Content = "line one" }
		);
        yield return SwaggerExample.Create(
			"Várias Linhas",
			new CreateGoogleDriveFileIn {  Name = "ManyLines.txt", Content = "line one\nline two\nline three" }
		);
    }
}
