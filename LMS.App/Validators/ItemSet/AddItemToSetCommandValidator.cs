using FluentValidation;
using LMS.App.Features.ItemSets.Commands.AddItemToSet;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ItemSet;

public class AddItemToSetCommandValidator : AbstractValidator<AddItemToSetCommand>
{
    public AddItemToSetCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.SetId).GreaterThan(0).WithMessage(localizer["SetIdPositiveNumber"]);
        RuleFor(x => x.ItemId).GreaterThan(0).WithMessage(localizer["ItemIdPositiveNumber"]);
        RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["UserIdRequiredOwnership"]);
    }
}