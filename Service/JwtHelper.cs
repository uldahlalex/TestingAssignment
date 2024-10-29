using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Service;

public static class JwtHelper
{
    public static string GenerateToken( string userId, string jwtKey, string issuer, string audience)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));
        
        var credentials = new SigningCredentials(
            key, 
            SecurityAlgorithms.HmacSha512); 

        var token = new JwtSecurityToken(
            issuer:  issuer,
            audience: audience,
            claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            },
            expires: DateTime.Now.AddYears(1000),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}