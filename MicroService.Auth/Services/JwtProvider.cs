﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MicroService.Auth.DTOs;
using MicroService.Auth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MicroService.Auth.Services;

public sealed class JwtProvider(IOptions<JwtOptions> jwtOptions)
{
    public LoginResponseDto GenerateToken(Guid userId, string userName, List<string> roles)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        string secretKey = jwtOptions.Value.SecretKey;
        string refreshToken = Guid.CreateVersion7().ToString();
        DateTime expires = DateTime.Now.AddDays(1);

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurity = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials
        );

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        string token = handler.WriteToken(jwtSecurity);

        LoginResponseDto response = new(token, refreshToken, expires.AddDays(1), userId);        
        return response;
    }
}