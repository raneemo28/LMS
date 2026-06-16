using LMS.Domain.Constants;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IStringLocalizer<ErrorMessages> localizer)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _localizer = localizer;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        var result = new List<UserSummaryDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserSummaryDto(user.Id, $"{user.FirstName} {user.LastName}", user.Email ?? string.Empty, user.IsActive, user.CreatedAt, roles.FirstOrDefault() ?? "Member"));
        }
        return Ok(result);
    }

    [HttpGet("by-role")]
    public async Task<IActionResult> GetByRole([FromQuery] string role)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(role);
        var result = usersInRole.Select(u => new UserSummaryDto(u.Id, $"{u.FirstName} {u.LastName}", u.Email ?? string.Empty, u.IsActive, u.CreatedAt, role));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound(new { Message = _localizer["ResourceNotFound"] });
        
        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new UserSummaryDto(user.Id, $"{user.FirstName} {user.LastName}", user.Email ?? string.Empty, user.IsActive, user.CreatedAt, roles.FirstOrDefault() ?? "Member"));
    }

    [HttpPost("{id}/promote-librarian")]
    public async Task<IActionResult> PromoteToLibrarian(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound(new { Message = _localizer["ResourceNotFound"] });
        
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(Roles.Admin)) return BadRequest(new { Message = _localizer["NotAuthorizedUpdateItem"] });
        
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await _userManager.AddToRoleAsync(user, Roles.Librarian);
        return result.Succeeded ? Ok(new { Success = true, Message = $"{user.Email} is now a Librarian." }) : BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
    }

    [HttpPost("{id}/demote-member")]
    public async Task<IActionResult> DemoteToMember(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound(new { Message = _localizer["ResourceNotFound"] });
        
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(Roles.Admin)) return BadRequest(new { Message = _localizer["NotAuthorizedUpdateItem"] });
        
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await _userManager.AddToRoleAsync(user, Roles.Member);
        return result.Succeeded ? Ok(new { Success = true, Message = $"{user.Email} has been demoted to Member." }) : BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
    }

    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound(new { Message = _localizer["ResourceNotFound"] });
        
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(Roles.Admin)) return BadRequest(new { Message = _localizer["NotAuthorizedUpdateItem"] });
        
        user.IsActive = false;
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded ? Ok(new { Success = true, Message = $"{user.Email} has been deactivated." }) : BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
    }

    [HttpPost("{id}/reactivate")]
    public async Task<IActionResult> Reactivate(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound(new { Message = _localizer["ResourceNotFound"] });
        
        user.IsActive = true;
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded ? Ok(new { Success = true, Message = $"{user.Email} has been reactivated." }) : BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
    }
}

public record UserSummaryDto(string Id, string FullName, string Email, bool IsActive, DateTime CreatedAt, string Role);