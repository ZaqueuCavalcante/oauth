using OAuth.Draw.Security;

namespace OAuth.Draw.Features.CreateUser;

public class CreateUserService(DrawDbContext ctx, IPasswordHasher hasher) : IDrawService
{
    public async Task<OneOf<CreateUserOut, DrawError>> Create(CreateUserIn data)
    {
        var user = new DrawUser(data.Name, data.Email);

        var passwordHash = hasher.Hash(user.Id, user.Email, data.Password);
        user.SetPasswordHash(passwordHash);

        ctx.Add(user);

        try
        {
            await ctx.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("ix_users_email")) return new EmailAlreadyUsed();
        }

        return user.ToOut();
    }
}
