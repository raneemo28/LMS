using AutoMapper;
using LMS.App.DTOs.Auth;
using LMS.App.Interface;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace LMS.App.Features.Register.Commands;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private const string RoleName = "Member";

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
        var user = _mapper.Map<ApplicationUser>(request.Data)
            ?? throw new InvalidOperationException("Failed to map application user.");

        var result = await _userManager.CreateAsync(user, request.Data.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse
            {
                Email   = request.Data.Email,
                Success = false,
                Message = $"Registration failed: {errors}"
            };
        }

        // Not necessary for now but kept for future role support
        if (!string.IsNullOrWhiteSpace(RoleName) && await _roleManager.RoleExistsAsync(RoleName))
            await _userManager.AddToRoleAsync(user, RoleName);

        var authResponse = _mapper.Map<AuthResponse>(user);
        var token = _jwtService.GenerateJwtToken(user, RoleName);

        return authResponse with
        {
            Token   = token,
            Role    = RoleName,
            Success = true,
            Message = "Registration successful."
        };
    }
}