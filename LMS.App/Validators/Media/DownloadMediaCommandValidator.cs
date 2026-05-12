using FluentValidation;
using LMS.App.Features.Medias.Commands.DownloadMedia;

namespace LMS.App.Validators.Media;

public class DownloadMediaCommandValidator : AbstractValidator<DownloadMediaCommand>
{
    public DownloadMediaCommandValidator()
    {
        RuleFor(x => x.MediaId)
            .GreaterThan(0).WithMessage("Valid Media ID is required to download the file.");
    }
}
