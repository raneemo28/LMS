using FluentValidation;
using LMS.App.Features.Resources.Commands.AddValue;
using LMS.Domain.Constants;

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
            .NotEmpty().WithMessage("Type is required.")
            .Must(t => new[] { SystemConstants.TypeText, SystemConstants.TypeUri, SystemConstants.TypeResource }.Contains(t))
            .WithMessage("Type must be 'text', 'uri', or 'resource'.");

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
