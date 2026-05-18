using FluentValidation;
using LMS.App.Features.Media.Commands.CreateMediaCommand;

namespace LMS.App.Validators.Media;

public class CreateMediaCommandValidator : AbstractValidator<CreateMediaCommand>
{
    public CreateMediaCommandValidator()
    {
        RuleFor(x => x.Dto.FileName)
            .NotEmpty().WithMessage("FileName is required.")
            .MaximumLength(255).WithMessage("FileName is too long.");

        RuleFor(x => x.Dto.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");

        RuleFor(x => x.Dto.Values)
            .NotNull().WithMessage("Values list cannot be null.");

        RuleForEach(x => x.Dto.Values).ChildRules(value =>
        {
            value.RuleFor(v => v.PropertyId).GreaterThan(0);
            value.RuleFor(v => v.Type).NotEmpty();
        });
    }
}
