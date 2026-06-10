using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;

namespace LMS.App.Validators.ResourceTemplates;

public class UpdateResourceTemplateCommandValidator : AbstractValidator<UpdateResourceTemplateCommand>
{
    public UpdateResourceTemplateCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Template ID must be greater than 0.");

        RuleFor(x => x.Dto).NotNull().WithMessage("Template data is required.");

        RuleFor(x => x.Dto.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MaximumLength(255).WithMessage("Label must not exceed 255 characters.");
    }
}