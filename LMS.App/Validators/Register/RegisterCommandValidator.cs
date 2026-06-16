using FluentValidation;
using LMS.App.Features.Register.Commands;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["EmailRequired"])
            .EmailAddress().WithMessage(localizer["ValidEmailRequired"]);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(localizer["FirstNameRequired"])
            .MaximumLength(50).WithMessage(localizer["FirstNameMaxLength"]);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(localizer["LastNameRequired"])
            .MaximumLength(50).WithMessage(localizer["LastNameMaxLength"]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["PasswordRequired"])
            .MinimumLength(8).WithMessage(localizer["PasswordMinLength"])
            .Matches("[A-Z]").WithMessage(localizer["PasswordRequireUppercase"])
            .Matches("[a-z]").WithMessage(localizer["PasswordRequireLowercase"])
            .Matches("[0-9]").WithMessage(localizer["PasswordRequireDigit"])
            .Matches("[^a-zA-Z0-9]").WithMessage(localizer["PasswordRequireNonAlphanumeric"]);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage(localizer["PasswordsDoNotMatch"]);
    }
}