namespace OAuth.DrawApp.Features.CreateUser;

[ApiController]
[Consumes("application/json"), Produces("application/json")]
public class CreateUserController(CreateUserService service) : ControllerBase
{
    /// <summary>
    /// Criar usuário
    /// </summary>
    [HttpPost("users")]
    [ProducesResponseType(typeof(CreateUserOut), 200)]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Create([FromBody] CreateUserIn data)
    {
        var result = await service.Create(data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestsExamples : IExamplesProvider<CreateUserIn>
{
    CreateUserIn IExamplesProvider<CreateUserIn>.GetExamples()
    {
        return new CreateUserIn(
            "João da Silva",
            "joao.da.silva@gmail.com",
            "bfD43ae@8c46cb9fd18");
    }
}

internal class ResponseExamples : IMultipleExamplesProvider<CreateUserOut>
{
    public IEnumerable<SwaggerExample<CreateUserOut>> GetExamples()
    {
        yield return SwaggerExample.Create(
			"CreateUserOut",
			new CreateUserOut
			{
				Id = Guid.NewGuid(),
				Name = "João da Silva",
				Email = "joao.da.silva@gmail.com",
			}
		);
    }
}
