namespace OAuth.DrawApp.Database;

public static class DbContextExtensions
{
    public static void ResetDb(this DrawAppDbContext ctx)
    {
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();
    }
}
