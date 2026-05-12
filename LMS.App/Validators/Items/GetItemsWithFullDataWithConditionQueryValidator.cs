using FluentValidation;
using LMS.App.Features.Items.Queries.GetItemsWithFullDataWithConditionAsync;

namespace LMS.App.Validators.Items;

public class GetItemsWithFullDataWithConditionQueryValidator : AbstractValidator<GetItemsWithFullDataWithConditionQuery>
{
    public GetItemsWithFullDataWithConditionQueryValidator()
    {
        RuleFor(x => x.Filter)
            .NotNull().WithMessage("Filter expression is required.");
    }
}
