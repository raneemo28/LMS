using LMS.App.DTOs.users;
using LMS.Domain.Constants;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Users.Commands;

public class DemoteToMemberHandler : IRequestHandler<DemoteToMemberCommand, UserOperationResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public DemoteToMemberHandler(UserManager<ApplicationUser> userManager, IStringLocalizer<ErrorMessages> localizer)
    {
        _userManager = userManager;
        _localizer = localizer;
    }

    public async Task<UserOperationResult> Handle(DemoteToMemberCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null) return new UserOperationResult(false, _localizer["ResourceNotFound"]);

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(Roles.Admin)) return new UserOperationResult(false, _localizer["NotAuthorizedUpdateItem"]);

        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await _userManager.AddToRoleAsync(user, Roles.Member);

        if (result.Succeeded)
        {
            return new UserOperationResult(true, string.Format(_localizer["UserDemotedMember"], user.Email));
        }

        return new UserOperationResult(false, _localizer["FailedToDemoteUser"], result.Errors.Select(e => e.Description));
    }
}
