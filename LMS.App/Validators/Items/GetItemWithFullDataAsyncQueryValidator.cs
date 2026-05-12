using FluentValidation;
using LMS.App.Features.Items.Queries.GetItemWithFullDataAsync;

namespace LMS.App.Validators.Items;
public class GetItemWithFullDataAsyncQueryValidator : AbstractValidator<GetItemWithFullDataAsyncQuery>
{
    public GetItemWithFullDataAsyncQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Item ID is required.")
            .GreaterThan(0).WithMessage("Item ID must be a valid positive integer.");
    }
}
