using FluentValidation;
using LMS.App.Features.Media.Commands.CreateMediaCommand;

namespace LMS.App.Validators.Media;

public class CreateMediaCommandValidator : AbstractValidator<CreateMediaCommand>
{
    public CreateMediaCommandValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("FileName is required.")
            .MaximumLength(15).WithMessage("FileName is too long.");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");

        RuleFor(x => x.Values)
            .NotNull().WithMessage("Values list cannot be null.");

        RuleForEach(x => x.Values).ChildRules(value =>
        {
            value.RuleFor(v => v.PropertyId).GreaterThan(0);
            value.RuleFor(v => v.Type).NotEmpty();
        });
    }
}
