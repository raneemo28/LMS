using FluentValidation;
using LMS.App.Features.Vocabularies.Commands.UpdateVocabulary;

namespace LMS.App.Validators.Vocabulary;

public class UpdateVocabularyCommandValidator : AbstractValidator<UpdateVocabularyCommand>
{
    public UpdateVocabularyCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("A valid Vocabulary ID is required.");

        RuleFor(x => x.Dto.Prefix)
            .NotEmpty().WithMessage("Prefix is required.")
            .MaximumLength(20).WithMessage("Prefix cannot exceed 20 characters.");

        RuleFor(x => x.Dto.NamespaceUri)
            .NotEmpty().WithMessage("NamespaceUri is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("NamespaceUri must be a valid absolute URI.");

        RuleFor(x => x.Dto.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MaximumLength(100).WithMessage("Label cannot exceed 100 characters.");
    }
}