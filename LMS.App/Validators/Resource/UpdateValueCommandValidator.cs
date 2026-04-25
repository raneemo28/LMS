using FluentValidation;
using LMS.Application.Features.Resources.Commands.UpdateValue;

namespace LMS.Application.Validators.Resources;

public class UpdateValueCommandValidator : AbstractValidator<UpdateValueCommand>
{
    public UpdateValueCommandValidator()
    {
        RuleFor(x => x.ValueId)
            .GreaterThan(0).WithMessage("ValueId must be a valid ID.");

        RuleFor(x => x.ResourceId)
            .GreaterThan(0).WithMessage("ResourceId must be a valid ID.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.");
            
        RuleFor(x => x.ValueUri)
                    .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                    .When(x => !string.IsNullOrEmpty(x.ValueUri))
                    .WithMessage("The provided URI is not in a valid format.");

        RuleFor(x => x.Language)
            .MaximumLength(5)
            .When(x => x.Language != null);

        RuleFor(x => x.ValueText)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.ValueText));

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.ValueText) ||
                       !string.IsNullOrWhiteSpace(x.ValueUri) ||
                       x.ValueResourceId.HasValue)
            .WithMessage("Update must include at least one value (Text, Uri, or ResourceId).");
    }
}