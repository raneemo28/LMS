using FluentValidation;
using LMS.App.Features.Medias.Commands.DownloadMedia;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Media;

public class DownloadMediaCommandValidator : AbstractValidator<DownloadMediaCommand>
{
    public DownloadMediaCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.MediaId).GreaterThan(0).WithMessage(localizer["ValidMediaIdDownload"]);
    }
}