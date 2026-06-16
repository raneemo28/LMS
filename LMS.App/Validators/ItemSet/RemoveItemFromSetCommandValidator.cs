using FluentValidation;
using LMS.App.Features.ItemSets.Commands.RemoveItemFromSet;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ItemSets;

public class RemoveItemFromSetCommandValidator : AbstractValidator<RemoveItemFromSetCommand>
{
    public RemoveItemFromSetCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.SetId).GreaterThan(0).WithMessage(localizer["SetIdPositiveNumber"]);
        RuleFor(x => x.ItemId).GreaterThan(0).WithMessage(localizer["ItemIdPositiveNumber"]);
        RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["UserIdRequiredAuthority"]);
    }
}