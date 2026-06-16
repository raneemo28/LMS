using FluentValidation;
using LMS.App.Features.Items.Queries.GetItemWithFullDataAsync;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Items;

public class GetItemWithFullDataAsyncQueryValidator : AbstractValidator<GetItemWithFullDataAsyncQuery>
{
    public GetItemWithFullDataAsyncQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(localizer["ItemIdRequired"])
            .GreaterThan(0).WithMessage(localizer["ItemIdPositiveInteger"]);
    }
}