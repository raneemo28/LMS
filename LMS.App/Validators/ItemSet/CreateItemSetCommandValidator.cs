using FluentValidation;
using LMS.App.Features.ItemSets.Commands.CreateItemSets;

namespace LMS.App.Validators.ItemSets;

public class CreateItemSetCommandValidator : AbstractValidator<CreateItemSetCommand>
{
    public CreateItemSetCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(20).WithMessage("Title cannot exceed 20 characters.");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");

        RuleForEach(x => x.Values).ChildRules(value =>
        {
            value.RuleFor(v => v.PropertyId)
                .GreaterThan(0).WithMessage("PropertyId must be valid.");

            value.RuleFor(v => v.Type)
                .NotEmpty().WithMessage("Value Type is required.");
        });
    }
}
