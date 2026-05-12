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

namespace LMS.App.Features.Register.Commands;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    string roleName = "Member";

    public RegisterHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IMapper mapper,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<ApplicationUser>(request.Data);
        var result = await _userManager.CreateAsync(user, request.Data.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse(string.Empty, string.Empty, request.Data.Email, string.Empty, false, $"Registration failed: {errors}");
        }

        //not necessary for now but i will keep it for future
        if (!string.IsNullOrWhiteSpace(roleName))
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        var authResponse = _mapper.Map<AuthResponse>(user);
        
        var token = _jwtService.GenerateJwtToken(user, roleName);
        
        // Set the token that was ignored by the mapper
        return authResponse with
        {
            Token = token,
            Role = roleName,
            Success = true,
            Message = "Registration successful."
        };
    }
}
