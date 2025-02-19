namespace OAuth.DrawApp.Features.CreateUser;

public class CreateUserOut
{
    /// <summary>
    /// Id do usuário criado
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }
}
