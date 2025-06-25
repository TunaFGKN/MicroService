using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MicroService.Auth.Services;

public sealed class JwtProvider
{
    public string GenerateToken()
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.CreateVersion7().ToString()),
            new Claim(ClaimTypes.Name, "Taner Saydam")
            // new Claim(ClaimTypes.Role, "Administrator")
        };

        string secretKey = "267d6e09-5c10-4330-bf5b-187338e1094bc7f7a6e3-7ef9-42df-8fc2-42f169694f59de9dc4b5-9597-4697-865a-5d3b54c856074aea4512-caef-4157-8127-e440bff760099a189c36-dd76-404e-ae3d-dca48fb6645d";
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurity = new JwtSecurityToken(
            issuer: "Tuna",
            audience: "Customers",
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials
        );

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        string token = handler.WriteToken(jwtSecurity);

        return token;
    }
}
