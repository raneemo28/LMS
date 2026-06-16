using FluentValidation;
using LMS.App.Features.Media.Commands.UploadMediaFile;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Media;

public class UploadMediaFileCommandValidator : AbstractValidator<UploadMediaFileCommand>
{
    public UploadMediaFileCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.MediaId).GreaterThan(0).WithMessage(localizer["MediaIdValid"]);
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage(localizer["FileContentEmpty"])
            .Must(x => x.Length <= 10 * 1024 * 1024).WithMessage(localizer["FileSizeExceedsLimit"]);
        RuleFor(x => x.FileName).NotEmpty().WithMessage(localizer["FileNameRequired"]);
        RuleFor(x => x.MimeType).NotEmpty().WithMessage(localizer["MimeTypeRequired"]);
    }
}