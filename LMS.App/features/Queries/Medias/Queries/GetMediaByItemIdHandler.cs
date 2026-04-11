using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Media.Queries.GetMediaByItemId;

public class GetMediaByItemIdHandler : IRequestHandler<GetMediaByItemIdQuery, IEnumerable<LMS.Domain.Entities.Media>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMediaByItemIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<LMS.Domain.Entities.Media>> Handle(GetMediaByItemIdQuery request, CancellationToken cancellationToken)
    {
        var mediaList = await _unitOfWork.Media.GetMediaByItemIdAsync(request.ItemId);

        return mediaList ?? Enumerable.Empty<LMS.Domain.Entities.Media>();
    }
}