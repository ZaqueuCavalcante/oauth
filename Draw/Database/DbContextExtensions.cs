namespace OAuth.Draw.Database;

public static class DbContextExtensions
{
    public static void ResetDb(this DrawDbContext ctx)
    {
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();
    }
}
