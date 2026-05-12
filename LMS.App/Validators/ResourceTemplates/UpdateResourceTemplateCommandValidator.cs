using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;

namespace LMS.App.Validators.ResourceTemplates;

public class UpdateResourceTemplateCommandValidator : AbstractValidator<UpdateResourceTemplateCommand>
{
    public UpdateResourceTemplateCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Template ID must be a valid positive number.");

        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("Template label cannot be empty.")
            .MaximumLength(100).WithMessage("Label is too long (max 100 chars).");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description is too long (max 500 chars).");
    }
}
