using FluentValidation;
using LMS.App.Features.Medias.Queries.GetMediaWithMetadata;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Media;

public class GetMediaWithMetadataQueryValidator : AbstractValidator<GetMediaWithMetadataQuery>
{
    public GetMediaWithMetadataQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.MediaId).GreaterThan(0).WithMessage(localizer["MediaIdPositiveNumber"]);
    }
}