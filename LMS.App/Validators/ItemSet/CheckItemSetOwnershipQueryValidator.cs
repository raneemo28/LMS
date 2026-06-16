using FluentValidation;
using LMS.App.Features.Queries.ItemSets.CheckItemSetOwnership;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ItemSets;

public class CheckItemSetOwnershipQueryValidator : AbstractValidator<CheckItemSetOwnershipQuery>
{
    public CheckItemSetOwnershipQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer["ItemSetIdPositiveNumber"]);
        RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["UserIdNotEmpty"]);
    }
}