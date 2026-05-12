using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMS.App.DTOs.Auth;
using LMS.App.Features.Login.Command;
using LMS.App.Features.Register.Commands;
using LMS.App.Features.Logout.Command;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LMS.API.Controllers;
[ApiController][Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")] public async Task<IActionResult> Register([FromBody] RegisterRequest req) =>
        Ok(await _mediator.Send(new RegisterCommand(req)));
    [HttpPost("login")] public async Task<IActionResult> Login([FromBody] LogInRequest req) =>
        Ok(await _mediator.Send(new LoginCommand(req)));
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub")
                     ?? string.Empty;
        return Ok(await _mediator.Send(new LogoutCommand(userId)));
    }
}