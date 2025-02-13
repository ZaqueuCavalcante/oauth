namespace OAuth.Draw.Features.CreateUser;

public class CreateUserIn
{
    /// <summary>
    /// Nome
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Senha
    /// </summary>
    public string Password { get; set; }

    public CreateUserIn(
        string name,
        string email,
        string password
    ) {
        Name = name;
        Email = email;
        Password = password;
    }
}
