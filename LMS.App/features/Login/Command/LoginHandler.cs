using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using LMS.App.DTOs.Auth;
using LMS.App.Interface;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LMS.App.Features.Login.Command;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;

    public LoginHandler(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        IMapper mapper,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
        _jwtService = jwtService;
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
        var token = _jwtService.GenerateJwtToken(user, role);

        return authResponse with 
        { 
            Token = token, 
            Role = role, 
            Success = true, 
            Message = "Login successful." 
        };
    }
}
