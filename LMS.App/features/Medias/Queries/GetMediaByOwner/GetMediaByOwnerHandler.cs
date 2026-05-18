using MediatR;
using AutoMapper;
using LMS.App.DTOs.Media;
using LMS.Domain.Interfaces;

namespace LMS.App.features.Medias.Queries.GetMediaByOwner;
public class GetMediaByOwnerHandler : IRequestHandler<GetMediaByOwnerQuery, List<MediaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMediaByOwnerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<MediaDto>> Handle(GetMediaByOwnerQuery request, CancellationToken cancellationToken)
    {
        var results = await _unitOfWork.Media.GetMediaByOwnerAsync(request.UserId);
        return _mapper.Map<List<MediaDto>>(results.ToList()) ?? new List<MediaDto>();
    }
}
