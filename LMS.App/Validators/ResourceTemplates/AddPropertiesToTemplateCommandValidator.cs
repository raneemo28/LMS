using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.AddPropertiesToTemplate;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ResourceTemplates;

public class AddPropertiesToTemplateCommandValidator : AbstractValidator<AddPropertiesToTemplateCommand>
{
    public AddPropertiesToTemplateCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.TemplateId).GreaterThan(0).WithMessage(localizer["TemplateIdRequired"]);
        RuleFor(x => x.Properties).NotEmpty().WithMessage(localizer["PropertySpecifiedRequired"]);
        
        RuleForEach(x => x.Properties).ChildRules(prop =>
        {
            prop.RuleFor(x => x.PropertyId).GreaterThan(0).WithMessage(localizer["PropertyIdRequired"]);
            prop.RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0).WithMessage(localizer["DisplayOrderNotNegative"]);
            prop.RuleFor(x => x.AlternateLabel).MaximumLength(100).WithMessage(localizer["AlternateLabelTooLong"]);
        });
    }
}