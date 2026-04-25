using FluentValidation;
using LMS.Application.Features.Vocabularies.Commands.UpdateProperty;

namespace LMS.Application.Validators.Vocabularie;

public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
{
    public UpdatePropertyCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Property ID must be valid.");

        RuleFor(x => x.LocalName)
            .NotEmpty().WithMessage("LocalName is required.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("LocalName must be alphanumeric.");

        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("Label is required.")
            .MaximumLength(100);

        RuleFor(x => x.TermUri)
            .NotEmpty().WithMessage("TermUri is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("TermUri must be a valid absolute URL.");
    }
}