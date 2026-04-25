using FluentValidation;
using LMS.App.features.Items.Queries.GetItemsWithFullDataWithConditionAsync;

namespace LMS.App.features.Items.Queries.GetItemsWithFullDataWithConditionAsync;

public class GetItemsWithFullDataWithConditionQueryValidator : AbstractValidator<GetItemsWithFullDataWithConditionQuery>
{
    public GetItemsWithFullDataWithConditionQueryValidator()
    {
        RuleFor(x => x.Filter)
            .NotNull().WithMessage("Filter expression is required.");
    }
}