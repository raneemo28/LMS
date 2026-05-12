using MediatR;
using LMS.Domain.Interfaces;
using LMS.App.DTOs.Media;

namespace LMS.App.features.Medias.Queries.GetMediaByOwner;
public record GetMediaByOwnerQuery(string UserId) : IRequest<List<MediaDto>>;
