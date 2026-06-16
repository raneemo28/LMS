using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.UpdatePropertyInTemplate;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ResourceTemplates;

public class UpdatePropertyInTemplateCommandValidator : AbstractValidator<UpdatePropertyInTemplateCommand>
{
    public UpdatePropertyInTemplateCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.TemplateId).GreaterThan(0).WithMessage(localizer["TemplateIdRequired"]);
        RuleFor(x => x.PropertyId).GreaterThan(0).WithMessage(localizer["PropertyIdRequired"]);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0).WithMessage(localizer["DisplayOrderGreaterEqualZero"]);
        RuleFor(x => x.AlternateLabel).MaximumLength(100).WithMessage(localizer["AlternateLabelTooLong"]);
    }
}