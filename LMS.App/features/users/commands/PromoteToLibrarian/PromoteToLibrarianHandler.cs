using LMS.App.DTOs.users;
using LMS.Domain.Constants;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Users.Commands;

public class PromoteToLibrarianHandler : IRequestHandler<PromoteToLibrarianCommand, UserOperationResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public PromoteToLibrarianHandler(UserManager<ApplicationUser> userManager, IStringLocalizer<ErrorMessages> localizer)
    {
        _userManager = userManager;
        _localizer = localizer;
    }

    public async Task<UserOperationResult> Handle(PromoteToLibrarianCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null) return new UserOperationResult(false, _localizer["ResourceNotFound"]);

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(Roles.Admin)) return new UserOperationResult(false, _localizer["NotAuthorizedUpdateItem"]);

        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await _userManager.AddToRoleAsync(user, Roles.Librarian);

        if (result.Succeeded)
        {
            return new UserOperationResult(true, string.Format(_localizer["UserPromotedLibrarian"], user.Email));
        }

        return new UserOperationResult(false, _localizer["FailedToPromoteUser"], result.Errors.Select(e => e.Description));
    }
}
