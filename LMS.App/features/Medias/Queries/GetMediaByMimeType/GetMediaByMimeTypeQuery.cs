using MediatR;
using LMS.App.DTOs.Media;
namespace LMS.App.features.Medias.Queries.GetMediaByMimeType;
public record GetMediaByMimeTypeQuery(string MimeType) : IRequest<List<MediaDto>>;