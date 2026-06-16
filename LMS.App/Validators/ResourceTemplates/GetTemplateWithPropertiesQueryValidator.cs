using FluentValidation;
using LMS.App.Features.ResourceTemplates.Queries.GetTemplateWithProperties;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ResourceTemplates;

public class GetTemplateWithPropertiesQueryValidator : AbstractValidator<GetTemplateWithPropertiesQuery>
{
    public GetTemplateWithPropertiesQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer["TemplateIdPositiveInteger"]);
    }
}