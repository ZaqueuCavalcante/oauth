namespace OAuth.Draw.Features.CreateUser;

public class DrawUserConfig : IEntityTypeConfiguration<DrawUser>
{
    public void Configure(EntityTypeBuilder<DrawUser> user)
    {
        user.ToTable("users");

        user.HasKey(u => u.Id);
        user.Property(u => u.Id).ValueGeneratedNever();

        user.HasIndex(u => u.Email).IsUnique();
    }
}
