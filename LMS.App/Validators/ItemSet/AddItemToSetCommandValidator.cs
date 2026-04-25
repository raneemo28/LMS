using FluentValidation;
using LMS.App.Features.ItemSets.Commands.AddItemToSet;
namespace LMS.App.Validators.ItemSet;
public class AddItemToSetCommandValidator : AbstractValidator<AddItemToSetCommand>
{
    public AddItemToSetCommandValidator()
    {
        RuleFor(x => x.SetId)
            .GreaterThan(0).WithMessage("Set ID must be a valid positive number.");

        RuleFor(x => x.ItemId)
            .GreaterThan(0).WithMessage("Item ID must be a valid positive number.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required to verify ownership.");
    }
}
