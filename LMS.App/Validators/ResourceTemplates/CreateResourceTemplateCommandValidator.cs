using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;

namespace LMS.App.Validators.ResourceTemplates;
    
public class CreateResourceTemplateCommandValidator : AbstractValidator<CreateResourceTemplateCommand>
{
    public CreateResourceTemplateCommandValidator()
    {
        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("Template label is required.")
            .MaximumLength(100).WithMessage("Label cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description is too long.");
    }
}
