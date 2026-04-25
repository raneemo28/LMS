using FluentValidation;
using LMS.Application.Features.ItemSets.Queries.GetItemSetWithMembers;

namespace LMS.Application.Validators.ItemSets;
public class GetItemSetWithMembersQueryValidator : AbstractValidator<GetItemSetWithMembersQuery>
{
    public GetItemSetWithMembersQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ItemSet ID is required.")
            .GreaterThan(0).WithMessage("ItemSet ID must be a valid positive number.");
    }
}