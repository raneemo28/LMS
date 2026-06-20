using LMS.App.DTOs.users;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Users.Commands;

public class ReactivateUserHandler : IRequestHandler<ReactivateUserCommand, UserOperationResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public ReactivateUserHandler(UserManager<ApplicationUser> userManager, IStringLocalizer<ErrorMessages> localizer)
    {
        _userManager = userManager;
        _localizer = localizer;
    }

    public async Task<UserOperationResult> Handle(ReactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null) return new UserOperationResult(false, _localizer["ResourceNotFound"]);

        user.IsActive = true;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            return new UserOperationResult(true, string.Format(_localizer["UserReactivated"], user.Email));
        }

        return new UserOperationResult(false, _localizer["FailedToReactivateUser"], result.Errors.Select(e => e.Description));
    }
}
