using FluentValidation;
using LMS.App.Features.Users.Queries;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Users;

public class GetUsersByRoleQueryValidator : AbstractValidator<GetUsersByRoleQuery>
{
    public GetUsersByRoleQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Role).NotEmpty().WithMessage(localizer["RoleRequired"]);
    }
}
