using OAuth.DrawApp.Configs;
using OAuth.DrawApp.Security;
using System.Security.Claims;
using OAuth.DrawApp.Features.CreateUser;

namespace OAuth.DrawApp.Features.Login;

public class LoginService(DrawAppDbContext ctx, IPasswordHasher hasher) : IDrawAppService
{
    public async Task<OneOf<ClaimsPrincipal, DrawAppError>> Login(LoginIn data)
    {
        var user = await ctx.Users.FirstOrDefaultAsync(u => u.Email == data.Email);
        if (user == null) return new UserNotFound();

        var success = hasher.Verify(user.Id, user.Email, data.Password, user.PasswordHash);
        if (!success) return new WrongPassword();

        return GetClaimsPrincipal(user);
    }

    public ClaimsPrincipal GetClaimsPrincipal(DrawAppUser user)
    {
        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("name", user.Name),
            new("email", user.Email),
        };

        var claimsIdentity = new ClaimsIdentity(claims, AuthenticationConfigs.DrawAppCookieScheme);
        return new ClaimsPrincipal(claimsIdentity);
    }
}
