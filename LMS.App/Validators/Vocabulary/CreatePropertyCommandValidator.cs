using FluentValidation;
using LMS.App.Features.Vocabularies.Commands.CreateProperty;

namespace LMS.App.Validators.Vocabulary;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator()
    {
        RuleFor(x => x.VocabularyId)
            .GreaterThan(0).WithMessage("VocabularyId is required.");

        RuleFor(x => x.LocalName)
            .NotEmpty().WithMessage("LocalName is required.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("LocalName must be alphanumeric (no spaces).");

        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MaximumLength(100).WithMessage("Label is too long.");

        RuleFor(x => x.TermUri)
            .NotEmpty().WithMessage("TermUri is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("TermUri must be a valid absolute URL (e.g., http://schema.org/name).");
    }
}
