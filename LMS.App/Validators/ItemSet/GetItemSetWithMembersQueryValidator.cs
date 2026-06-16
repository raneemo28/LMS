using FluentValidation;
using LMS.App.Features.ItemSets.Queries.GetItemSetWithMembers;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ItemSets;

public class GetItemSetWithMembersQueryValidator : AbstractValidator<GetItemSetWithMembersQuery>
{
    public GetItemSetWithMembersQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(localizer["ItemSetIdRequired"])
            .GreaterThan(0).WithMessage(localizer["ItemSetIdPositiveNumber"]);
    }
}