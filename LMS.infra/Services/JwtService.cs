using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LMS.App.Interface;
using LMS.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LMS.Infra.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(ApplicationUser user, string role)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]
                ?? throw new InvalidOperationException("JWT Key missing")));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Role, role),
            new("firstName", user.FirstName),
            new("lastName", user.LastName),
        };

        var token = new JwtSecurityToken(
            issuer:   jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims:   claims,
            expires:  DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}