using FluentValidation;
using LMS.App.Features.Media.Queries.GetMediaByItemId;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Media;

public class GetMediaByItemIdQueryValidator : AbstractValidator<GetMediaByItemIdQuery>
{
    public GetMediaByItemIdQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.ItemId).GreaterThan(0).WithMessage(localizer["ItemIdPositiveNumber"]);
    }
}