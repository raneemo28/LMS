using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;
using LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;

namespace LMS.API.Controllers;
[ApiController][Route("api/[controller]")][Authorize]
public class WorkflowCheckController : ControllerBase
{
    private readonly IMediator _mediator;
    public WorkflowCheckController(IMediator mediator) => _mediator = mediator;

    [HttpGet("verify")]
    public IActionResult VerifyToken() => Ok(new {
        Message = "JWT Valid",
        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub"),
        Email = User.FindFirstValue(ClaimTypes.Email),
        Role = User.FindFirstValue(ClaimTypes.Role),
        Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
    });

    [HttpPost("create-template")]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateResourceTemplateCommand cmd) =>
        CreatedAtAction(nameof(VerifyToken), new { id = await _mediator.Send(cmd) });
}