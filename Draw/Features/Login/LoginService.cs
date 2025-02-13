using OAuth.Draw.Security;
using OAuth.Draw.Features.GenerateJWT;

namespace OAuth.Draw.Features.Login;

public class LoginService(DrawDbContext ctx, IPasswordHasher hasher, GenerateJWTService service) : IDrawService
{
    public async Task<OneOf<LoginOut, DrawError>> Login(LoginIn data)
    {
        var user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == data.Email);
        if (user == null) return new UserNotFound();

        var success = hasher.Verify(user.Id, user.Email, data.Password, user.PasswordHash);
        if (!success) return new WrongPassword();

        return new LoginOut { AccessToken = service.Generate(user) };
    }
}
