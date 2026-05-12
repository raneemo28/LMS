using MediatR;
using LMS.App.DTOs.Media;

namespace LMS.App.Features.Medias.Commands.DownloadMedia;

public class DownloadMediaCommand : IRequest<DownloadMediaResult>
{
    public int MediaId { get; set; }
}
