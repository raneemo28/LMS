using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.AddPropertiesToTemplate;

namespace LMS.App.Validators.ResourceTemplates;

public class AddPropertiesToTemplateCommandValidator : AbstractValidator<AddPropertiesToTemplateCommand>
{
    public AddPropertiesToTemplateCommandValidator()
    {
        RuleFor(x => x.TemplateId)
            .GreaterThan(0).WithMessage("TemplateId is required.");

        RuleFor(x => x.Properties)
            .NotEmpty().WithMessage("At least one property must be specified.");

        RuleForEach(x => x.Properties).ChildRules(prop =>
        {
            prop.RuleFor(x => x.PropertyId)
                .GreaterThan(0).WithMessage("PropertyId is required.");

            prop.RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder cannot be negative.");

            prop.RuleFor(x => x.AlternateLabel)
                .MaximumLength(100).WithMessage("Alternate label is too long.");
        });
    }
}
