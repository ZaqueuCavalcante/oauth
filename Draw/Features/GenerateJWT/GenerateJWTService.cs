using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OAuth.Draw.Features.CreateUser;
using System.IdentityModel.Tokens.Jwt;

namespace OAuth.Draw.Features.GenerateJWT;

public class GenerateJWTService(IConfiguration configuration) : IDrawService
{
    public string Generate(DrawUser user)
    {
        var claims = new List<Claim>
        {
            new("jti", Guid.NewGuid().ToString()),
            new("sub", user.Id.ToString()),
            new("name", user.Name),
            new("email", user.Email),
        };

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var settings = configuration.Auth();
        var key = Encoding.ASCII.GetBytes(settings.SecurityKey);
        var expirationTime = settings.ExpirationTimeInMinutes;
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = settings.Issuer,
            Subject = identityClaims,
            Audience = settings.Audience,
            SigningCredentials = signingCredentials,
            Expires = DateTime.UtcNow.AddMinutes(expirationTime),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
