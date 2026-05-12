using FluentValidation;
using LMS.App.Features.Queries.ItemSets.CheckItemSetOwnership;

namespace LMS.App.Validators.ItemSets;

public class CheckItemSetOwnershipQueryValidator : AbstractValidator<CheckItemSetOwnershipQuery>
{
    public CheckItemSetOwnershipQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ItemSet ID must be a valid positive number.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID cannot be empty.");
    }
}
