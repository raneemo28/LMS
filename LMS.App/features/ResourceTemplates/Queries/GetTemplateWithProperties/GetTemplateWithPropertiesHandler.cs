using LMS.Domain.Interfaces;
using MediatR;
using AutoMapper;
using LMS.App.DTOs.ResourceTemplate;

namespace LMS.App.Features.ResourceTemplates.Queries.GetTemplateWithProperties;

public class GetTemplateWithPropertiesHandler : IRequestHandler<GetTemplateWithPropertiesQuery, ResourceTemplateDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTemplateWithPropertiesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResourceTemplateDto?> Handle(GetTemplateWithPropertiesQuery request, CancellationToken cancellationToken)
    {
        var template = await _unitOfWork.ResourceTemplates.GetTemplateWithPropertiesAsync(request.Id);
        if (template == null) return null;
        var dto = _mapper.Map<ResourceTemplateDto>(template);
        if (dto==null) throw new Exception("mapping Recource Tamplate failed");
        return dto;
    }
}
