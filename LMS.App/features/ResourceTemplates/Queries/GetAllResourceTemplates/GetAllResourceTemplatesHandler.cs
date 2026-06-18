using MediatR;
using AutoMapper;
using LMS.Domain.Interfaces;
using LMS.App.DTOs.ResourceTemplate;

namespace LMS.App.Features.ResourceTemplates.Queries.GetAllResourceTemplates;

public class GetAllResourceTemplatesHandler : IRequestHandler<GetAllResourceTemplatesQuery, IEnumerable<ResourceTemplateDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllResourceTemplatesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ResourceTemplateDto>> Handle(GetAllResourceTemplatesQuery request, CancellationToken cancellationToken)
    {
        var templates = await _unitOfWork.ResourceTemplates.GetAllAsync();
        return _mapper.Map<IEnumerable<ResourceTemplateDto>>(templates);
    }
}