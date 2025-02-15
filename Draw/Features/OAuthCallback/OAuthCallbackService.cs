namespace OAuth.Draw.Features.OAuthCallback;

public class OAuthCallbackService(DrawDbContext ctx) : IDrawService
{
    public async Task<OneOf<string, DrawError>> Callback(OAuthCallbackIn data)
    {
        var user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == data.State);
        


        return user.Email;
    }
}
