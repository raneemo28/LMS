using FluentValidation;
using LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ResourceTemplates;

public class CreateResourceTemplateCommandValidator : AbstractValidator<CreateResourceTemplateCommand>
{
    public CreateResourceTemplateCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Dto).NotNull().WithMessage(localizer["TemplateDataRequired"]);
        RuleFor(x => x.Dto.Label).NotEmpty().WithMessage(localizer["LabelRequired"]).MaximumLength(255).WithMessage(localizer["LabelMaxLength"]);
        RuleFor(x => x.Dto.Description).MaximumLength(1000).WithMessage(localizer["DescriptionMaxLength"]).When(x => x.Dto.Description != null);
    }
}