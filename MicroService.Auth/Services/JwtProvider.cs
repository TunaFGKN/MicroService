using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MicroService.Auth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MicroService.Auth.Services;

public sealed class JwtProvider(IOptions<JwtOptions> jwtOptions)
{
    public string GenerateToken()
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.CreateVersion7().ToString()),
            new Claim(ClaimTypes.Name, "Taner Saydam")
            // new Claim(ClaimTypes.Role, "Administrator")
        };

        string secretKey = jwtOptions.Value.SecretKey;
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurity = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials
        );

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        string token = handler.WriteToken(jwtSecurity);

        return token;
    }
}