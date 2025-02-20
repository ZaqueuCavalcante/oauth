using OAuth.DrawApp.Features.CreateUser;
using OAuth.DrawApp.Features.CreateUserOAuthToken;

namespace OAuth.DrawApp.Database;

public class DrawAppDbContext(DbContextOptions<DrawAppDbContext> options, IConfiguration configuration) : DbContext(options)
{
    public DbSet<DrawAppUser> Users { get; set; }
    public DbSet<UserOAuthToken> Tokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseNpgsql(configuration.Database().ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("drawapp");

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }
}
