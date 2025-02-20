namespace OAuth.DrawApp.Features.CreateUser;

public class DrawAppUserConfig : IEntityTypeConfiguration<DrawAppUser>
{
    public void Configure(EntityTypeBuilder<DrawAppUser> user)
    {
        user.ToTable("users");

        user.HasKey(u => u.Id);
        user.Property(u => u.Id).ValueGeneratedNever();

        user.HasIndex(u => u.Email).IsUnique();
    }
}
