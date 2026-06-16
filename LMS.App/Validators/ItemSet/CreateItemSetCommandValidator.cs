using FluentValidation;
using LMS.App.Features.ItemSets.Commands.CreateItemSets;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ItemSet;

public class CreateItemSetCommandValidator : AbstractValidator<CreateItemSetCommand>
{
    public CreateItemSetCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.OwnerId).NotEmpty().WithMessage(localizer["OwnerIdRequired"]);
        RuleFor(x => x.Dto).NotNull().WithMessage(localizer["ItemSetDataRequired"]);
        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage(localizer["TitleRequired"])
            .MaximumLength(255).WithMessage(localizer["TitleMaxLength"]);
        RuleFor(x => x.Dto.Description)
            .MaximumLength(1000).WithMessage(localizer["DescriptionMaxLength"])
            .When(x => x.Dto.Description != null);
    }
}