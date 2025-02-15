using OAuth.Draw.Configs;
using OAuth.Draw.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace OAuth.Draw.Features.Login;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class LoginController(DrawDbContext ctx, IPasswordHasher hasher) : ControllerBase
{
    /// <summary>
    /// 🔓 Login
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ErrorOut), 400)]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Login([FromBody] LoginIn data)
    {
        var user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == data.Email);
        if (user == null) return BadRequest(new UserNotFound());

        var success = hasher.Verify(user.Id, user.Email, data.Password, user.PasswordHash);
        if (!success) return BadRequest(new WrongPassword());

        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("name", user.Name),
            new("email", user.Email),
        };

        var claimsIdentity = new ClaimsIdentity(claims, AuthenticationConfigs.DrawCookieScheme);

        await HttpContext.SignInAsync(
            AuthenticationConfigs.DrawCookieScheme, 
            new ClaimsPrincipal(claimsIdentity));

        return LocalRedirect("/");
    }
}

internal class RequestsExamples : IMultipleExamplesProvider<LoginIn>
{
    public IEnumerable<SwaggerExample<LoginIn>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"Cliente - João da Silva",
			new LoginIn(
                "joao.da.silva@gmail.com",
                "bfD43ae@8c46cb9fd18")
		);
        yield return SwaggerExample.Create(
			"Lojista - Gilbirdelson Lanches",
			new LoginIn(
                "gilbirdelson.lanches@gmail.com",
                "dc9ab8a59@60b44edbcd71ba5Ec1a0f")
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
