using MediatR;
using AutoMapper;
using LMS.Domain.Interfaces;
using LMS.App.DTOs.Media;

namespace LMS.App.Features.Medias.Queries.GetAllMedias;

public class GetAllMediasHandler : IRequestHandler<GetAllMediasQuery, IEnumerable<MediaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMediasHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MediaDto>> Handle(GetAllMediasQuery request, CancellationToken cancellationToken)
    {
        var medias = await _unitOfWork.Media.GetAllAsync();
        return _mapper.Map<IEnumerable<MediaDto>>(medias);
    }
}