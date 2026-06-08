using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;

namespace LMS.App.Validators.ResourceTemplates;

public class CreateResourceTemplateCommandValidator : AbstractValidator<CreateResourceTemplateCommand>
{
    public CreateResourceTemplateCommandValidator()
    {
        RuleFor(x => x.Dto).NotNull().WithMessage("Template data is required.");

        RuleFor(x => x.Dto.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MaximumLength(255).WithMessage("Label must not exceed 255 characters.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .When(x => x.Dto.Description != null);
    }
}