namespace OAuth.DrawApp.Features.CreateUserOAuthToken;

public class UserOAuthTokenConfig : IEntityTypeConfiguration<UserOAuthToken>
{
    public void Configure(EntityTypeBuilder<UserOAuthToken> token)
    {
        token.ToTable("user_oauth_tokens");

        token.HasKey(t => t.Id);
        token.Property(t => t.Id).ValueGeneratedNever();
    }
}
