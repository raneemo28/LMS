using MediatR;

namespace LMS.Application.Features.Media.Commands.DownloadMedia;

public class DownloadMediaCommand : IRequest<DownloadMediaResult>
{
    public int MediaId { get; set; }
}