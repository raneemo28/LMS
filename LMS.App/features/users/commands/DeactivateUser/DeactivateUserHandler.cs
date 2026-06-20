using LMS.App.DTOs.users;
using LMS.Domain.Constants;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Users.Commands;

public class DeactivateUserHandler : IRequestHandler<DeactivateUserCommand, UserOperationResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public DeactivateUserHandler(UserManager<ApplicationUser> userManager, IStringLocalizer<ErrorMessages> localizer)
    {
        _userManager = userManager;
        _localizer = localizer;
    }

    public async Task<UserOperationResult> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null) return new UserOperationResult(false, _localizer["ResourceNotFound"]);

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(Roles.Admin)) return new UserOperationResult(false, _localizer["NotAuthorizedUpdateItem"]);

        user.IsActive = false;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return new UserOperationResult(true, string.Format(_localizer["UserDeactivated"], user.Email));
        }

        return new UserOperationResult(false, _localizer["FailedToDeactivateUser"], result.Errors.Select(e => e.Description));
    }
}
