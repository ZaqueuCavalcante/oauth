namespace OAuth.Draw.Features.CreateUserOAuthToken;

public class UserOAuthToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Value { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public UserOAuthToken(Guid userId, string value)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Value = value;
        CreatedAt = DateTime.Now;
    }
}
