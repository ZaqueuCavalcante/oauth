using OAuth.Draw.Features.CreateUser;
using OAuth.Draw.Features.CreateUserOAuthToken;

namespace OAuth.Draw.Database;

public class DrawDbContext(DbContextOptions<DrawDbContext> options, IConfiguration configuration) : DbContext(options)
{
    public DbSet<DrawUser> Users { get; set; }
    public DbSet<UserOAuthToken> Tokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseNpgsql(configuration.Database().ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("draw");

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }
}
