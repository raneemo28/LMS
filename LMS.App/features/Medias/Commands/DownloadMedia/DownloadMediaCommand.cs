using MediatR;
using LMS.Domain.Entities;

namespace LMS.Application.Features.Media.Commands.DownloadMedia;

public record DownloadMediaCommand(int MediaId) : IRequest<LMS.Domain.Entities.Media?>;