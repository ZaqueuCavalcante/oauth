using Microsoft.AspNetCore.Authorization;

namespace OAuth.Draw.Features.GetUserData;

[Authorize]
[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class GetUserDataController(DrawDbContext ctx) : ControllerBase
{
    /// <summary>
    /// Dados do usuário
    /// </summary>
    [HttpGet("users/data")]
    [ProducesResponseType(typeof(GetUserDataOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> GetUserData()
    {
        var user = await ctx.Users.FirstAsync(x => x.Id == User.Id());

        var result = new GetUserDataOut
        {
            Name = user.Name,
            Email = user.Email,
            GoogleDriveEnabled = User.GoogleDriveEnabled(),
        };

        return Ok(result);
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<GetUserDataOut>
{
    public IEnumerable<SwaggerExample<GetUserDataOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"GetUserDataOut",
			new GetUserDataOut
			{
				Name = "João da Silva",
				Email = "joao.da.silva@gmail.com",
                GoogleDriveEnabled = false,
			}
		);
    }
}
