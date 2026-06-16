using FluentValidation;
using LMS.App.features.Medias.Queries.GetMediaByMimeType;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Media;

public class GetMediaByMimeTypeQueryValidator : AbstractValidator<GetMediaByMimeTypeQuery>
{
    public GetMediaByMimeTypeQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.MimeType)
            .NotEmpty().WithMessage(localizer["MimeTypeRequired"])
            .Must(x => x.Contains("/") || x.Contains("%2F", StringComparison.OrdinalIgnoreCase))
            .WithMessage(localizer["InvalidMimeType"]);
    }
}