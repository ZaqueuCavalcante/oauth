using OAuth.Draw.Configs;
using Microsoft.AspNetCore.Authentication;

namespace OAuth.Draw.Features.Login;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class LoginController(LoginService service) : ControllerBase
{
    /// <summary>
    /// Login
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Login([FromBody] LoginIn data)
    {
        var result = await service.Login(data);

        if (result.IsError()) return BadRequest(result.GetError());

        await HttpContext.SignInAsync(
            AuthenticationConfigs.DrawCookieScheme, 
            result.GetSuccess());

        return LocalRedirect("/");
    }
}

internal class RequestsExamples : IMultipleExamplesProvider<LoginIn>
{
    public IEnumerable<SwaggerExample<LoginIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"João da Silva",
			new LoginIn(
                "joao.da.silva@gmail.com",
                "bfD43ae@8c46cb9fd18")
		);
    }
}

internal class ErrorsExamples : IMultipleExamplesProvider<ErrorOut>
{
    public IEnumerable<SwaggerExample<ErrorOut>> GetExamples()
    {
        yield return new UserNotFound().ToExampleErrorOut();
        yield return new WrongPassword().ToExampleErrorOut();
    }
}
