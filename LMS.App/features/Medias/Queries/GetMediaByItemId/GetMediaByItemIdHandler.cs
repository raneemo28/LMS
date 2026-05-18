using AutoMapper;
using LMS.App.DTOs.Media;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Media.Queries.GetMediaByItemId;

public class GetMediaByItemIdHandler : IRequestHandler<GetMediaByItemIdQuery, List<MediaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMediaByItemIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<MediaDto>> Handle(GetMediaByItemIdQuery request, CancellationToken cancellationToken)
    {
        var mediaList = await _unitOfWork.Media.GetMediaByItemIdAsync(request.ItemId);

        return _mapper.Map<List<MediaDto>>(mediaList) ?? new List<MediaDto>();
    }
}
