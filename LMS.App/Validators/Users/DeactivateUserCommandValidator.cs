using FluentValidation;
using LMS.App.Features.Users.Commands;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Users;

public class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["UserIdRequired"]);
    }
}
