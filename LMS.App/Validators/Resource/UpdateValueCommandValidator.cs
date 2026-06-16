using FluentValidation;
using LMS.App.Features.Resources.Commands.UpdateValue;
using LMS.Domain.Constants;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Resource;

public class UpdateValueCommandValidator : AbstractValidator<UpdateValueCommand>
{
    public UpdateValueCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.ResourceId).GreaterThan(0).WithMessage(localizer["ResourceIdGreaterThanZero"]);
        RuleFor(x => x.ValueId).GreaterThan(0).WithMessage(localizer["ValueIdGreaterThanZero"]);
        RuleFor(x => x.Dto).NotNull().WithMessage(localizer["ValueDataRequired"]);
        
        RuleFor(x => x.Dto.Type)
            .NotEmpty().WithMessage(localizer["TypeRequired"])
            .Must(t => new[] { SystemConstants.TypeText, SystemConstants.TypeUri, SystemConstants.TypeResource }.Contains(t))
            .WithMessage(localizer["TypeMustBeTextUriResource"]);
            
        RuleFor(x => x.Dto)
            .Must(d => !string.IsNullOrEmpty(d.ValueText) || d.ValueResourceId.HasValue)
            .WithMessage(localizer["MustProvideValueTextOrResource"]);
    }
}