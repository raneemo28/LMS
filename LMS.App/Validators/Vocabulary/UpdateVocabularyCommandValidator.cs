using FluentValidation;
using LMS.App.Features.Vocabularies.Commands.UpdateVocabulary;

namespace LMS.App.Validators.Vocabulary;

public class UpdateVocabularyCommandValidator : AbstractValidator<UpdateVocabularyCommand>
{
    public UpdateVocabularyCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Vocabulary ID is required.");

        RuleFor(x => x.Prefix)
            .NotEmpty().WithMessage("Prefix is required.")
            .Matches(@"^[a-z]+$").WithMessage("Prefix must be lowercase alphabetic characters.");

        RuleFor(x => x.NamespaceUri)
            .NotEmpty().WithMessage("NamespaceUri is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("NamespaceUri must be a valid absolute URL.");

        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MaximumLength(100);
    }
}
