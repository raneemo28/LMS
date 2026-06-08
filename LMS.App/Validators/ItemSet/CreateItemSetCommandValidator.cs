using FluentValidation;
using LMS.App.Features.ItemSets.Commands.CreateItemSets;

namespace LMS.App.Validators.ItemSet;

public class CreateItemSetCommandValidator : AbstractValidator<CreateItemSetCommand>
{
    public CreateItemSetCommandValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");

        RuleFor(x => x.Dto).NotNull().WithMessage("ItemSet data is required.");

        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

        RuleFor(x => x.Dto.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .When(x => x.Dto.Description != null);
    }
}