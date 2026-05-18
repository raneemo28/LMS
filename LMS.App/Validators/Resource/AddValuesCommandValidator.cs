using FluentValidation;
using LMS.App.Features.Resources.Commands.AddValues;
using LMS.Domain.Constants;
using System.Linq;

namespace LMS.App.Validators.Resources;

public class AddValuesCommandValidator : AbstractValidator<AddValuesCommand>
{
    public AddValuesCommandValidator()
    {
        RuleFor(x => x.ResourceId)
            .GreaterThan(0).WithMessage("ResourceId must be valid.");

        RuleFor(x => x.Values)
            .NotEmpty().WithMessage("Values list cannot be empty.");

        RuleForEach(x => x.Values).ChildRules(val =>
        {
            val.RuleFor(v => v.PropertyId)
                .GreaterThan(0).WithMessage("PropertyId must be valid.");

            val.RuleFor(v => v.Type)
                .NotEmpty().WithMessage("Type is required.")
                .Must(t => new[] { SystemConstants.TypeText, SystemConstants.TypeUri, SystemConstants.TypeResource }.Contains(t))
                .WithMessage("Type must be 'text', 'uri', or 'resource'.");

            val.RuleFor(v => v.Language)
                .MaximumLength(5)
                .When(v => v.Language != null);

            val.RuleFor(v => v.ValueText)
                .MaximumLength(100)
                .When(v => !string.IsNullOrEmpty(v.ValueText));

            val.RuleFor(v => v)
                .Must(v => !string.IsNullOrWhiteSpace(v.ValueText) ||
                           v.ValueResourceId.HasValue)
                .WithMessage("At least one value (Text or ResourceId) must be provided.");
        });
    }
}
