using FluentValidation;
using LMS.App.Features.Media.Queries.GetMediaByItemId;

namespace LMS.App.Validators.Media;

public class GetMediaByItemIdQueryValidator : AbstractValidator<GetMediaByItemIdQuery>
{
    public GetMediaByItemIdQueryValidator()
    {
        RuleFor(x => x.ItemId)
            .GreaterThan(0).WithMessage("Item ID must be a valid positive number.");
    }
}
