using FluentValidation;

using LMS.App.Features.Items.Commands.CreateItem;

namespace LMS.App.Validators.Items;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.TemplateId)
                    .NotEmpty().WithMessage("TemplateId is required.")
                    .GreaterThan(0).WithMessage("TemplateId must be greater than 0.");
        RuleFor(x => x.OwnerId)
                    .NotEmpty().WithMessage("OwnerId is required.")
                    .NotNull().WithMessage("OwnerId cannot be null.");
        RuleFor(x => x.Values)
            .NotNull().WithMessage("Values list cannot be null.")
            .Must(v => v != null && v.Any()).WithMessage("At least one value is required.");
        RuleForEach(x => x.Values).ChildRules(value =>
        {
            value.RuleFor(v => v.PropertyId)
                .GreaterThan(0).WithMessage("PropertyId must be a valid ID.");

            value.RuleFor(v => v.Type)
                .NotEmpty().WithMessage("Type is required.");

            value.RuleFor(v => v)
                .Must(v => !string.IsNullOrEmpty(v.ValueText) || 
                           !string.IsNullOrEmpty(v.ValueUri) || 
                           v.ValueResourceId.HasValue)
                .WithMessage("Value must contain either Text, Uri, or a Resource ID.");
        });
    }
}
