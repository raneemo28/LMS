using FluentValidation;
using LMS.App.features.Medias.Queries.GetMediaByMimeType;

namespace LMS.App.Validators.Media;

public class GetMediaByMimeTypeQueryValidator : AbstractValidator<GetMediaByMimeTypeQuery>
{
    public GetMediaByMimeTypeQueryValidator()
    {
        RuleFor(x => x.MimeType)
            .NotEmpty().WithMessage("MimeType is required (e.g., 'image/png').")
            .Must(x => x.Contains("/")).WithMessage("Invalid MimeType format.");
    }
}