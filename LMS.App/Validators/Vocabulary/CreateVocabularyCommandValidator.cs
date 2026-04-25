using FluentValidation;
using LMS.Application.Features.Vocabularies.Commands.CreateVocabulary;

namespace LMS.Application.Validators.Vocabularie;

public class CreateVocabularyCommandValidator : AbstractValidator<CreateVocabularyCommand>
{
    public CreateVocabularyCommandValidator()
    {
        RuleFor(x => x.Prefix)
            .NotEmpty().WithMessage("Prefix is required.")
            .MaximumLength(10).WithMessage("Prefix should be short (e.g., 'dc', 'schema').")
            .Matches(@"^[a-z]+$").WithMessage("Prefix must be lowercase alphabetic characters only.");

        RuleFor(x => x.NamespaceUri)
            .NotEmpty().WithMessage("NamespaceUri is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("NamespaceUri must be a valid absolute URL.");

        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MaximumLength(100).WithMessage("Label is too long.");
    }
}