using FluentValidation;
using LMS.App.Features.Medias.Queries.GetMediaWithMetadata;

namespace LMS.App.Validators.Media;

public class GetMediaWithMetadataQueryValidator : AbstractValidator<GetMediaWithMetadataQuery>
{
    public GetMediaWithMetadataQueryValidator()
    {
        RuleFor(x => x.MediaId)
            .GreaterThan(0).WithMessage("Media ID must be a valid positive number.");
    }
}
