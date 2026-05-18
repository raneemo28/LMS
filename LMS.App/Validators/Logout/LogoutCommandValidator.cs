using FluentValidation;
using LMS.App.Features.Logout.Command;

namespace LMS.App.Validators.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required for logout.");
    }
}
