// FILE: LMS.API/Controllers/UsersController.cs
using LMS.Domain.Constants;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Controllers;

/// <summary>
/// Admin-only controller for managing users (view all, promote to Librarian,
/// deactivate/reactivate, and seed the default admin account).
/// </summary>
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // ─── GET /api/users ───────────────────────────────────────────────────────
    /// <summary>
    /// Returns all users with their assigned role.
    /// Useful for the admin dashboard to see Members, Librarians, and Admins.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        var result = new List<UserSummaryDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserSummaryDto(
                user.Id,
                $"{user.FirstName} {user.LastName}",
                user.Email ?? string.Empty,
                user.IsActive,
                user.CreatedAt,
                roles.FirstOrDefault() ?? "Member"
            ));
        }

        return Ok(result);
    }

    // ─── GET /api/users/by-role?role=Librarian ────────────────────────────────
    /// <summary>
    /// Filter users by role: Admin | Librarian | Member
    /// </summary>
    [HttpGet("by-role")]
    public async Task<IActionResult> GetByRole([FromQuery] string role)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(role);

        var result = usersInRole.Select(u => new UserSummaryDto(
            u.Id,
            $"{u.FirstName} {u.LastName}",
            u.Email ?? string.Empty,
            u.IsActive,
            u.CreatedAt,
            role
        ));

        return Ok(result);
    }

    // ─── GET /api/users/{id} ──────────────────────────────────────────────────
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound(new { Message = $"User {id} not found." });

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new UserSummaryDto(
            user.Id,
            $"{user.FirstName} {user.LastName}",
            user.Email ?? string.Empty,
            user.IsActive,
            user.CreatedAt,
            roles.FirstOrDefault() ?? "Member"
        ));
    }

    // ─── POST /api/users/{id}/promote-librarian ───────────────────────────────
    /// <summary>
    /// Promotes an existing Member to the Librarian role.
    /// The admin can call this after the user has already registered.
    /// </summary>
    [HttpPost("{id}/promote-librarian")]
    public async Task<IActionResult> PromoteToLibrarian(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound(new { Message = $"User {id} not found." });

        // Remove existing roles first so no user has two roles simultaneously
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(Roles.Admin))
            return BadRequest(new { Message = "Cannot change an Admin's role." });

        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await _userManager.AddToRoleAsync(user, Roles.Librarian);

        return result.Succeeded
            ? Ok(new { Success = true, Message = $"{user.Email} is now a Librarian." })
            : BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
    }

    // ─── POST /api/users/{id}/demote-member ───────────────────────────────────
    /// <summary>
    /// Demotes a Librarian back to Member.
    /// </summary>
    [HttpPost("{id}/demote-member")]
    public async Task<IActionResult> DemoteToMember(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound(new { Message = $"User {id} not found." });

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(Roles.Admin))
            return BadRequest(new { Message = "Cannot demote an Admin." });

        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await _userManager.AddToRoleAsync(user, Roles.Member);

        return result.Succeeded
            ? Ok(new { Success = true, Message = $"{user.Email} has been demoted to Member." })
            : BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
    }

    // ─── POST /api/users/{id}/deactivate ─────────────────────────────────────
    /// <summary>
    /// Soft-deactivates a user (sets IsActive = false).
    /// The user will still exist in the database but can be filtered out on login.
    /// </summary>
    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound(new { Message = $"User {id} not found." });

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(Roles.Admin))
            return BadRequest(new { Message = "Cannot deactivate an Admin account." });

        user.IsActive = false;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Ok(new { Success = true, Message = $"{user.Email} has been deactivated." })
            : BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
    }

    // ─── POST /api/users/{id}/reactivate ─────────────────────────────────────
    [HttpPost("{id}/reactivate")]
    public async Task<IActionResult> Reactivate(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return NotFound(new { Message = $"User {id} not found." });

        user.IsActive = true;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Ok(new { Success = true, Message = $"{user.Email} has been reactivated." })
            : BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
    }
}

// ─── Response DTO ─────────────────────────────────────────────────────────────
public record UserSummaryDto(
    string Id,
    string FullName,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    string Role
);
