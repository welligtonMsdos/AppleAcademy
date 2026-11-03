using AppleAcademy.Application.Dtos;
using AppleAcademy.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppleAcademy.Infrastructure.Services;

public class TokenService : ITokenService
{
    public async Task<string> GenerateToken(UserDataLoginDto userDataLoginDto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var keyVault = Environment.GetEnvironmentVariable("JwtSettings__Key");

        ArgumentNullException.ThrowIfNull(keyVault);

        var key = Encoding.ASCII.GetBytes(keyVault);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
               new Claim("guid", userDataLoginDto.Guid.ToString()),
               new Claim("email", userDataLoginDto.Email),
               new Claim("name", userDataLoginDto.Name),
               new Claim("role", userDataLoginDto.PerfilName)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = "http://localhost:5000",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
