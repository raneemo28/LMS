using FluentValidation;
using LMS.App.features.Medias.Queries.GetMediaByOwner;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Media;

public class GetMediaByOwnerQueryValidator : AbstractValidator<GetMediaByOwnerQuery>
{
    public GetMediaByOwnerQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(localizer["UserIdRequiredMedia"])
            .MinimumLength(5).WithMessage(localizer["InvalidUserIdFormat"]);
    }
}