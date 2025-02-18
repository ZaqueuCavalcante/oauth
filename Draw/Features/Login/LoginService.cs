using OAuth.Draw.Configs;
using OAuth.Draw.Security;
using System.Security.Claims;
using OAuth.Draw.Features.CreateUser;

namespace OAuth.Draw.Features.Login;

public class LoginService(DrawDbContext ctx, IPasswordHasher hasher) : IDrawService
{
    public async Task<OneOf<ClaimsPrincipal, DrawError>> Login(LoginIn data)
    {
        var user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == data.Email);
        if (user == null) return new UserNotFound();

        var success = hasher.Verify(user.Id, user.Email, data.Password, user.PasswordHash);
        if (!success) return new WrongPassword();

        return GetClaimsPrincipal(user);
    }

    public ClaimsPrincipal GetClaimsPrincipal(DrawUser user)
    {
        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("name", user.Name),
            new("email", user.Email),
        };

        var claimsIdentity = new ClaimsIdentity(claims, AuthenticationConfigs.DrawCookieScheme);
        return new ClaimsPrincipal(claimsIdentity);
    }
}
