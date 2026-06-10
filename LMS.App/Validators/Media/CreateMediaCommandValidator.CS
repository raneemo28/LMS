using FluentValidation;
using LMS.App.Features.Media.Commands.CreateMediaCommand;

namespace LMS.App.Validators.Media;

public class CreateMediaCommandValidator : AbstractValidator<CreateMediaCommand>
{
    public CreateMediaCommandValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");

        RuleFor(x => x.Dto).NotNull().WithMessage("Media data is required.");

        RuleFor(x => x.Dto.FileName)
            .NotEmpty().WithMessage("FileName is required.")
            .MaximumLength(255).WithMessage("FileName must not exceed 255 characters.");
    }
}