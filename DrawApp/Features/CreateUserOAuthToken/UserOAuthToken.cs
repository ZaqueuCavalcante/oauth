namespace OAuth.DrawApp.Features.CreateUserOAuthToken;

public class UserOAuthToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string AccessToken { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public UserOAuthToken(Guid userId, string accessToken, string? refreshToken)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        CreatedAt = DateTime.Now;
    }
}
