using FluentValidation;
using LMS.Application.Features.Media.Commands.DownloadMedia;

namespace LMS.Application.Validators.Media;

public class DownloadMediaCommandValidator : AbstractValidator<DownloadMediaCommand>
{
    public DownloadMediaCommandValidator()
    {
        RuleFor(x => x.MediaId)
            .GreaterThan(0).WithMessage("Valid Media ID is required to download the file.");
    }
}