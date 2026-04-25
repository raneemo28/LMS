using FluentValidation;
using LMS.Application.Features.ItemSets.Commands.UpdateItemSets;

namespace LMS.Application.Validators.ItemSets;

public class UpdateItemSetCommandValidator : AbstractValidator<UpdateItemSetCommand>
{
    public UpdateItemSetCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Valid ItemSet ID is required for update.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(200).WithMessage("Title is too long.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required for security verification.");
            
        RuleForEach(x => x.Values).ChildRules(value =>
        {
            value.RuleFor(v => v.PropertyId).GreaterThan(0);
            value.RuleFor(v => v.Type).NotEmpty();
        });
    }
}