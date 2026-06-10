using FluentValidation;
using LMS.App.Features.Vocabularies.Commands.CreateProperty;

namespace LMS.App.Validators.Vocabulary;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator()
    {
        RuleFor(x => x.VocabularyId)
            .GreaterThan(0).WithMessage("VocabularyId must be valid.");

        RuleFor(x => x.Dto).NotNull().WithMessage("Property data is required.");

        RuleFor(x => x.Dto.LocalName)
            .NotEmpty().WithMessage("LocalName is required.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("LocalName must be alphanumeric.");

        RuleFor(x => x.Dto.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MaximumLength(255).WithMessage("Label must not exceed 255 characters.");

        RuleFor(x => x.Dto.TermUri)
            .NotEmpty().WithMessage("TermUri is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("TermUri must be a valid absolute URL.");
    }
}