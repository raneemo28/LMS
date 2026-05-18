using LMS.Domain.Interfaces;
using MediatR;
using AutoMapper;
using LMS.App.DTOs.Value;

namespace LMS.App.Features.Resources.Queries.GetResourceValues;

public class GetResourceValuesHandler : IRequestHandler<GetResourceValuesQuery, List<ResourceValueDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetResourceValuesHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ResourceValueDto>> Handle(GetResourceValuesQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Resources.GetResourceValuesAsync(request.ResourceId);
        return _mapper.Map<List<ResourceValueDto>>(result) ?? new List<ResourceValueDto>();
    }
}
