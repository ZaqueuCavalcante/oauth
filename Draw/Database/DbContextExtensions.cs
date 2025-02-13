namespace OAuth.Draw.Database;

public static class DbContextExtensions
{
    public static void ResetDb(this DrawDbContext ctx)
    {
        if (!Env.IsTesting())
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
        }
    }

    public static async Task ResetDbAsync(this DrawDbContext ctx)
    {
        if (Env.IsTesting())
        {
            await ctx.Database.EnsureDeletedAsync();
            await ctx.Database.EnsureCreatedAsync();
        }
    }
}
