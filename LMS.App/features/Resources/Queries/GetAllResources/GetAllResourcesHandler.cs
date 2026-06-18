using MediatR;
using AutoMapper;
using LMS.Domain.Interfaces;
using LMS.App.DTOs.Resource;

namespace LMS.App.Features.Resources.Queries.GetAllResources;

public class GetAllResourcesHandler : IRequestHandler<GetAllResourcesQuery, IEnumerable<ResourceDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllResourcesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ResourceDto>> Handle(GetAllResourcesQuery request, CancellationToken cancellationToken)
    {
        var resources = await _unitOfWork.Resources.GetAllAsync();
        return _mapper.Map<IEnumerable<ResourceDto>>(resources);
    }
}