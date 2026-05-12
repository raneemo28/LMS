using FluentValidation;
using LMS.App.Features.ItemSets.Commands.RemoveItemFromSet;

namespace LMS.App.Validators.ItemSets;

public class RemoveItemFromSetCommandValidator : AbstractValidator<RemoveItemFromSetCommand>
{
    public RemoveItemFromSetCommandValidator()
    {
        RuleFor(x => x.SetId)
            .GreaterThan(0).WithMessage("Set ID must be a valid positive number.");

        RuleFor(x => x.ItemId)
            .GreaterThan(0).WithMessage("Item ID must be a valid positive number.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required to verify your authority.");
    }
}
