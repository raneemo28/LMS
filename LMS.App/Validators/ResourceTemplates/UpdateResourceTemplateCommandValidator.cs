using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ResourceTemplates;

public class UpdateResourceTemplateCommandValidator : AbstractValidator<UpdateResourceTemplateCommand>
{
    public UpdateResourceTemplateCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer["TemplateIdGreaterThanZero"]);
        RuleFor(x => x.Dto).NotNull().WithMessage(localizer["TemplateDataRequired"]);
        RuleFor(x => x.Dto.Label).NotEmpty().WithMessage(localizer["LabelRequired"]).MaximumLength(255).WithMessage(localizer["LabelMaxLength"]);
    }
}