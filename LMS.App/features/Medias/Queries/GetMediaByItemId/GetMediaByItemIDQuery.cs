using LMS.App.DTOs.Media;
using MediatR;

namespace LMS.App.Features.Media.Queries.GetMediaByItemId;

public record GetMediaByItemIdQuery(
int ItemId
):IRequest<List<MediaDto>>;
