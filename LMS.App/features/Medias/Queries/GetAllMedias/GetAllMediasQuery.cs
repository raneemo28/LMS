using MediatR;
using LMS.App.DTOs.Media;

namespace LMS.App.Features.Medias.Queries.GetAllMedias;

public record GetAllMediasQuery : IRequest<IEnumerable<MediaDto>>;