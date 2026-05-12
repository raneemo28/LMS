using FluentValidation;
using LMS.App.Features.ResourceTemplates.Queries.GetTemplateWithProperties;

namespace LMS.App.Validators.ResourceTemplates;

public class GetTemplateWithPropertiesQueryValidator : AbstractValidator<GetTemplateWithPropertiesQuery>
{
    public GetTemplateWithPropertiesQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Template ID must be a valid positive integer.");
    }
}
