using FluentValidation;
using LMS.App.Features.Media.Commands.UploadMediaFile;

namespace LMS.App.Validators.Media;

public class UploadMediaFileCommandValidator : AbstractValidator<UploadMediaFileCommand>
{
    public UploadMediaFileCommandValidator()
    {
        RuleFor(x => x.MediaId)
            .GreaterThan(0).WithMessage("MediaId must be valid.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("File content cannot be empty.")
            .Must(x => x.Length <= 10 * 1024 * 1024)
            .WithMessage("File size exceeds the 10MB limit.");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("FileName is required.");

        RuleFor(x => x.MimeType)
            .NotEmpty().WithMessage("MimeType is required (e.g., image/jpeg).");
    }
}
