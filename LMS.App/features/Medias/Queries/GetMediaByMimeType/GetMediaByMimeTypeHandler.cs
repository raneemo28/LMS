using AutoMapper;
using MediatR;
using LMS.Domain.Interfaces;
using LMS.App.DTOs.Media;
namespace LMS.App.features.Medias.Queries.GetMediaByMimeType;

public class GetMediaByMimeTypeHandler : IRequestHandler<GetMediaByMimeTypeQuery, List<MediaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMediaByMimeTypeHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<MediaDto>> Handle(GetMediaByMimeTypeQuery request, CancellationToken cancellationToken)
    {
        
        var results = await _unitOfWork.Media.FindAsync(m => m.MimeType == request.MimeType);

        return _mapper.Map<List<MediaDto>>(results.ToList());
    }
}