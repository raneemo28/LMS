using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMS.App.DTOs.Auth;
using LMS.App.Features.Login.Command;
using LMS.App.Features.Register.Commands;
using LMS.App.Features.Logout.Command;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public AuthController(IMediator mediator, IStringLocalizer<ErrorMessages> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CustomRegisterRequest req)
    {
        var result = await _mediator.Send(new RegisterCommand(req.FirstName, req.LastName, req.MiddleName, req.Email, req.Password, req.ConfirmPassword, req.PhoneNumber));
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LogInRequest req)
    {
        var result = await _mediator.Send(new LoginCommand(req.Email, req.Password));
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized(new { Success = false, Message = _localizer["UserIdentityNotVerified"] });
            
        return Ok(await _mediator.Send(new LogoutCommand(userId)));
    }
}