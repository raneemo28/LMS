using MediatR;
using LMS.App.DTOs.Media;

namespace LMS.App.Features.Medias.Commands.DownloadMedia;

public record DownloadMediaCommand(int MediaId) : IRequest<DownloadMediaResult>;
