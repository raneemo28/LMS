using FluentValidation;
using LMS.App.Features.Resources.Commands.AddValues;
using LMS.Domain.Constants;
using System.Linq;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Resources;

public class AddValuesCommandValidator : AbstractValidator<AddValuesCommand>
{
    public AddValuesCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.ResourceId).GreaterThan(0).WithMessage(localizer["ResourceIdPositiveInteger"]);
        RuleFor(x => x.Values).NotEmpty().WithMessage(localizer["ValuesListNotEmpty"]);
        
        RuleForEach(x => x.Values).ChildRules(val =>
        {
            val.RuleFor(v => v.PropertyId).GreaterThan(0).WithMessage(localizer["PropertyIdValid"]);
            val.RuleFor(v => v.Type)
                .NotEmpty().WithMessage(localizer["TypeRequired"])
                .Must(t => new[] { SystemConstants.TypeText, SystemConstants.TypeUri, SystemConstants.TypeResource }.Contains(t))
                .WithMessage(localizer["TypeMustBeTextUriResource"]);
            val.RuleFor(v => v.Language).MaximumLength(5).When(v => v.Language != null);
            val.RuleFor(v => v.ValueText).MaximumLength(100).When(v => !string.IsNullOrEmpty(v.ValueText));
            val.RuleFor(v => v)
                .Must(v => !string.IsNullOrWhiteSpace(v.ValueText) || v.ValueResourceId.HasValue)
                .WithMessage(localizer["ValueTextOrResourceRequired"]);
        });
    }
}