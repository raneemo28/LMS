using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.AddPropertyToTemplate;

namespace LMS.App.Validators.ResourceTemplates;

public class AddPropertyToTemplateCommandValidator : AbstractValidator<AddPropertyToTemplateCommand>
{
    public AddPropertyToTemplateCommandValidator()
    {
        RuleFor(x => x.TemplateId)
            .GreaterThan(0).WithMessage("TemplateId is required.");

        RuleFor(x => x.PropertyId)
            .GreaterThan(0).WithMessage("PropertyId is required.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder cannot be negative.");

        RuleFor(x => x.AlternateLabel)
            .MaximumLength(100).WithMessage("Alternate label is too long.");
    }
}
