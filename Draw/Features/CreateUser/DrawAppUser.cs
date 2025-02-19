namespace OAuth.DrawApp.Features.CreateUser;

public class DrawAppUser
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private DrawAppUser() { }

    public DrawAppUser(
        string name,
        string email
    ) {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        CreatedAt = DateTime.Now;
    }

    public void SetPasswordHash(string passwordHash) => PasswordHash = passwordHash;

    public CreateUserOut ToOut() => new()
    {
        Id = Id,
        Name = Name,
        Email = Email,
    };
}
