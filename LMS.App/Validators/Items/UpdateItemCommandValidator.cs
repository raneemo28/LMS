using FluentValidation;
using LMS.App.Features.Items.Commands.UpdateItem;

namespace LMS.App.Validators.Items;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Item ID is required for update.")
            .GreaterThan(0).WithMessage("Item ID must be valid.");

        RuleFor(x => x.TemplateId)
            .GreaterThan(0).WithMessage("TemplateId must be a positive number.");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required for security checks.");

        RuleFor(x => x.Values)
            .NotNull().WithMessage("Values list cannot be null.")
            .Must(v => v.Any()).WithMessage("You must provide at least one value for the item.");

        RuleForEach(x => x.Values).ChildRules(value =>
        {
            value.RuleFor(v => v.PropertyId)
                .GreaterThan(0).WithMessage("PropertyId must be a valid ID.");

            value.RuleFor(v => v.Type)
                .NotEmpty().WithMessage("Value Type is required (e.g., Text, Image, etc.).");

            value.RuleFor(v => v)
                .Must(v => !string.IsNullOrEmpty(v.ValueText) || 
                           !string.IsNullOrEmpty(v.ValueUri) || 
                           v.ValueResourceId.HasValue)
                .WithMessage("Each entry must have at least a ValueText, ValueUri, or a ResourceId.");
        });
    }
}
