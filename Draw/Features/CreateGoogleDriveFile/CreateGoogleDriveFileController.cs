using OAuth.DrawApp.Auth;
using Microsoft.AspNetCore.Authorization;

namespace OAuth.DrawApp.Features.CreateGoogleDriveFile;

[ApiController]
[Authorize(Policies.GoogleDriveEnabled)]
[Consumes("application/json"), Produces("application/json")]
public class CreateGoogleDriveFileController(CreateGoogleDriveFileService service) : ControllerBase
{
    /// <summary>
    /// Criar arquivo
    /// </summary>
    [HttpPost("google-drive/files")]
    [ProducesResponseType(typeof(CreateGoogleDriveFileOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
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

internal class ResponseExamples : IMultipleExamplesProvider<CreateGoogleDriveFileOut>
{
    public IEnumerable<SwaggerExample<CreateGoogleDriveFileOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"CreateGoogleDriveFileOut",
			new CreateGoogleDriveFileOut
			{
				Id = "1Hcc9vb0bBYlvmfISmLbrCKM1gbLE-_d0",
			}
		);
    }
}
