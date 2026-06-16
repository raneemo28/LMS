using FluentValidation;
using LMS.App.Features.Login.Command;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["EmailRequired"])
            .EmailAddress().WithMessage(localizer["ValidEmailRequired"]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["PasswordRequired"])
            .MinimumLength(8).WithMessage(localizer["PasswordMinLength"])
            .Matches(@"[A-Z]").WithMessage(localizer["PasswordRequireUppercase"])
            .Matches(@"[a-z]").WithMessage(localizer["PasswordRequireLowercase"])
            .Matches(@"\d").WithMessage(localizer["PasswordRequireDigit"]);
    }
}