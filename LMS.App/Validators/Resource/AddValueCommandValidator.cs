using FluentValidation;
using LMS.App.Features.Resources.Commands.AddValue;

namespace LMS.App.Validators.Resources;

public class AddValueCommandValidator : AbstractValidator<AddValueCommand>
{
    public AddValueCommandValidator()
    {
        RuleFor(x => x.ResourceId)
            .GreaterThan(0).WithMessage("ResourceId must be valid.");

        RuleFor(x => x.PropertyId)
            .GreaterThan(0).WithMessage("PropertyId must be valid.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.");

        RuleFor(x => x.Language)
            .MaximumLength(5) 
            .When(x => x.Language != null);

        RuleFor(x => x.ValueText)
            .MaximumLength(100) 
            .When(x => !string.IsNullOrEmpty(x.ValueText));

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.ValueText) ||
                       x.ValueResourceId.HasValue)
            .WithMessage("At least one value (Text or ResourceId) must be provided.");
    }
}
