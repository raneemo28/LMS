using MediatR;

namespace LMS.Application.Features.Media.Queries.GetMediaByItemId;

public record GetMediaByItemIdQuery(int ItemId) : IRequest<IEnumerable<LMS.Domain.Entities.Media>>;