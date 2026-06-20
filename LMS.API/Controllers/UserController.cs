using LMS.Domain.Constants;
using LMS.Domain.Entities;
using LMS.App.DTOs.users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;
using MediatR;
using LMS.App.Features.Users.Commands;
using LMS.App.Features.Users.Queries;

namespace LMS.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public UsersController(IMediator mediator, IStringLocalizer<ErrorMessages> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }

    [HttpGet("by-role")]
    public async Task<IActionResult> GetByRole([FromQuery] string role)
    {
        var result = await _mediator.Send(new GetUsersByRoleQuery(role));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        if (result is null) return NotFound(new { Message = _localizer["ResourceNotFound"] });
        
        return Ok(result);
    }

    [HttpPost("{id}/promote-librarian")]
    public async Task<IActionResult> PromoteToLibrarian(string id)
    {
        var result = await _mediator.Send(new PromoteToLibrarianCommand(id));
        if (!result.Success && result.Message == "ResourceNotFound") return NotFound(new { Message = _localizer["ResourceNotFound"] });
        if (!result.Success && result.Message == "NotAuthorizedUpdateItem") return BadRequest(new { Message = _localizer["NotAuthorizedUpdateItem"] });

        return result.Success ? Ok(new { Success = true, Message = result.Message }) : BadRequest(new { Success = false, Errors = result.Errors });
    }

    [HttpPost("{id}/demote-member")]
    public async Task<IActionResult> DemoteToMember(string id)
    {
        var result = await _mediator.Send(new DemoteToMemberCommand(id));
        if (!result.Success && result.Message == "ResourceNotFound") return NotFound(new { Message = _localizer["ResourceNotFound"] });
        if (!result.Success && result.Message == "NotAuthorizedUpdateItem") return BadRequest(new { Message = _localizer["NotAuthorizedUpdateItem"] });

        return result.Success ? Ok(new { Success = true, Message = result.Message }) : BadRequest(new { Success = false, Errors = result.Errors });
    }

    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(string id)
    {
        var result = await _mediator.Send(new DeactivateUserCommand(id));
        if (!result.Success && result.Message == "ResourceNotFound") return NotFound(new { Message = _localizer["ResourceNotFound"] });
        if (!result.Success && result.Message == "NotAuthorizedUpdateItem") return BadRequest(new { Message = _localizer["NotAuthorizedUpdateItem"] });

        return result.Success ? Ok(new { Success = true, Message = result.Message }) : BadRequest(new { Success = false, Errors = result.Errors });
    }

    [HttpPost("{id}/reactivate")]
    public async Task<IActionResult> Reactivate(string id)
    {
        var result = await _mediator.Send(new ReactivateUserCommand(id));
        if (!result.Success && result.Message == "ResourceNotFound") return NotFound(new { Message = _localizer["ResourceNotFound"] });

        return result.Success ? Ok(new { Success = true, Message = result.Message }) : BadRequest(new { Success = false, Errors = result.Errors });
    }
}

