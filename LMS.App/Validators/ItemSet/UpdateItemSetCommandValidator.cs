using FluentValidation;
using LMS.App.Features.ItemSets.Commands.UpdateItemSets;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ItemSet;

public class UpdateItemSetCommandValidator : AbstractValidator<UpdateItemSetCommand>
{
    public UpdateItemSetCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["UserIdRequired"]);
        RuleFor(x => x.Dto).NotNull().WithMessage(localizer["ItemSetDataRequired"]);
        RuleFor(x => x.Dto.Id).GreaterThan(0).WithMessage(localizer["ItemSetIdGreaterThanZero"]);
        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage(localizer["TitleRequired"])
            .MaximumLength(255).WithMessage(localizer["TitleMaxLength"]);
    }
}