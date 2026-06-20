using AutoMapper;
using LMS.App.DTOs.Auth;
using LMS.App.Interface;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Register.Commands;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    private const string RoleName = "Member";

    public RegisterHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IMapper mapper,
        IJwtService jwtService,
        IStringLocalizer<ErrorMessages> localizer)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
        _jwtService = jwtService;
        _localizer = localizer;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<ApplicationUser>(request)
            ?? throw new InvalidOperationException(_localizer["FailedToMapApplicationUser"]);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse
            {
                Email   = request.Email,
                Success = false,
                Message = $"{_localizer["RegistrationFailed"]} {errors}"
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
            Message = _localizer["RegistrationSuccessful"]
        };
    }
}