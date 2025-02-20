using OAuth.DrawApp.Security;

namespace OAuth.DrawApp.Features.CreateUser;

public class CreateUserService(DrawAppDbContext ctx, IPasswordHasher hasher) : IDrawAppService
{
    public async Task<OneOf<CreateUserOut, DrawAppError>> Create(CreateUserIn data)
    {
        var user = new DrawAppUser(data.Name, data.Email);

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
