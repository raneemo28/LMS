using FluentValidation;
using LMS.App.Features.ItemSets.Commands.UpdateItemSets;

namespace LMS.App.Validators.ItemSet;

public class UpdateItemSetCommandValidator : AbstractValidator<UpdateItemSetCommand>
{
    public UpdateItemSetCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Dto).NotNull().WithMessage("ItemSet data is required.");

        RuleFor(x => x.Dto.Id)
            .GreaterThan(0).WithMessage("ItemSet ID must be greater than 0.");

        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
    }
}