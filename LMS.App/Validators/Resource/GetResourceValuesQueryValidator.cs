using FluentValidation;
using LMS.App.Features.Resources.Queries.GetResourceValues;

namespace LMS.App.Validators.Resources;

public class GetResourceValuesQueryValidator : AbstractValidator<GetResourceValuesQuery>
{
    public GetResourceValuesQueryValidator()
    {
        RuleFor(x => x.ResourceId).NotEmpty().WithMessage("Resource ID is required.")
            .GreaterThan(0).WithMessage("Resource ID must be a valid positive integer.");
    }
}
