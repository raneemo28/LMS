using FluentValidation;
using LMS.App.Features.Items.Commands.CreateItem;
using LMS.Domain.Constants;

namespace LMS.App.Validators.Items;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");

        RuleFor(x => x.Dto).NotNull().WithMessage("Item data is required.");

        RuleFor(x => x.Dto.TemplateId)
            .GreaterThan(0).WithMessage("TemplateId must be greater than 0.");

        RuleFor(x => x.Dto.Values)
            .NotNull().WithMessage("Values list cannot be null.")
            .Must(v => v != null && v.Any()).WithMessage("At least one value is required.");

        RuleForEach(x => x.Dto.Values).ChildRules(value =>
        {
            value.RuleFor(v => v.PropertyId)
                .GreaterThan(0).WithMessage("PropertyId must be a valid ID.");

            value.RuleFor(v => v.Type)
                .NotEmpty().WithMessage("Type is required.")
                .Must(t => new[] { SystemConstants.TypeText, SystemConstants.TypeUri, SystemConstants.TypeResource }.Contains(t))
                .WithMessage("Type must be 'text', 'uri', or 'resource'.");

            value.RuleFor(v => v)
                .Must(v => !string.IsNullOrEmpty(v.ValueText) ||
                           !string.IsNullOrEmpty(v.ValueUri) ||
                           v.ValueResourceId.HasValue)
                .WithMessage("Each value must have at least one of: ValueText, ValueUri, or ValueResourceId.");
        });
    }
}