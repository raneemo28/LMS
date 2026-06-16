using FluentValidation;
using LMS.App.Features.Items.Queries.GetItemsWithFullDataWithConditionAsync;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Items;

public class GetItemsWithFullDataWithConditionQueryValidator : AbstractValidator<GetItemsWithFullDataWithConditionQuery>
{
    public GetItemsWithFullDataWithConditionQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Filter).NotNull().WithMessage(localizer["FilterExpressionRequired"]);
    }
}