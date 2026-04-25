using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.UpdatePropertyInTemplate;

namespace LMS.App.Validators.ResourceTemplates;

public class UpdatePropertyInTemplateCommandValidator : AbstractValidator<UpdatePropertyInTemplateCommand>
{
    public UpdatePropertyInTemplateCommandValidator()
    {
        RuleFor(x => x.TemplateId)
            .GreaterThan(0).WithMessage("TemplateId is required.");

        RuleFor(x => x.PropertyId)
            .GreaterThan(0).WithMessage("PropertyId is required.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("DisplayOrder must be 0 or greater.");

        RuleFor(x => x.AlternateLabel)
            .MaximumLength(100).WithMessage("Alternate label is too long.");
    }
}