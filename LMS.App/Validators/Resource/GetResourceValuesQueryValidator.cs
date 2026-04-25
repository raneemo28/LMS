using FluentValidation;
using LMS.Application.Features.Resources.Queries.GetResourceValues;

namespace LMS.Application.Validators.Resources;

public class GetResourceValuesQueryValidator : AbstractValidator<GetResourceValuesQuery>
{
    public GetResourceValuesQueryValidator()
    {
        RuleFor(x => x.ResourceId).NotEmpty().WithMessage("Resource ID is required.")
            .GreaterThan(0).WithMessage("Resource ID must be a valid positive integer.");
    }
}