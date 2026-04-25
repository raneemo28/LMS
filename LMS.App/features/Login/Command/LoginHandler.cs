using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using LMS.App.DTO.Auth;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LMS.App.features.Login.Command;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public LoginHandler(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IMapper mapper)
    {
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Data.Email);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Data.Password))
        {
            return new AuthResponse(string.Empty, string.Empty, request.Data.Email, string.Empty, false, "Invalid email or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Member";

        var authResponse = _mapper.Map<AuthResponse>(user);
        var token = GenerateJwtToken(user, role);

        return authResponse with 
        { 
            Token = token, 
            Role = role, 
            Success = true, 
            Message = "Login successful." 
        };
    }

    private string GenerateJwtToken(ApplicationUser user, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var keyString = _configuration["Jwt:Key"] ;
        if (keyString == null) throw new Exception("failed to authenticate");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
