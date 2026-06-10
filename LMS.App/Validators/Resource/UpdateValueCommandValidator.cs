using FluentValidation;
using LMS.App.Features.Resources.Commands.UpdateValue;
using LMS.Domain.Constants;

namespace LMS.App.Validators.Resource;

public class UpdateValueCommandValidator : AbstractValidator<UpdateValueCommand>
{
    public UpdateValueCommandValidator()
    {
        RuleFor(x => x.ResourceId)
            .GreaterThan(0).WithMessage("ResourceId must be greater than 0.");

        RuleFor(x => x.ValueId)
            .GreaterThan(0).WithMessage("ValueId must be greater than 0.");

        RuleFor(x => x.Dto).NotNull().WithMessage("Value data is required.");

        RuleFor(x => x.Dto.Type)
            .NotEmpty().WithMessage("Type is required.")
            .Must(t => new[] { SystemConstants.TypeText, SystemConstants.TypeUri, SystemConstants.TypeResource }.Contains(t))
            .WithMessage("Type must be 'text', 'uri', or 'resource'.");

        RuleFor(x => x.Dto)
            .Must(d => !string.IsNullOrEmpty(d.ValueText) ||
                       d.ValueResourceId.HasValue)
            .WithMessage("Must provide either ValueText or ValueResourceId.");
    }
}