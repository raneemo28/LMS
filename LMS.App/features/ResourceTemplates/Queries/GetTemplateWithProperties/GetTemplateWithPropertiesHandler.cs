using LMS.Domain.Interfaces;
using MediatR;
using AutoMapper;
using LMS.App.DTOs.ResourceTemplate;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.ResourceTemplates.Queries.GetTemplateWithProperties;

public class GetTemplateWithPropertiesHandler : IRequestHandler<GetTemplateWithPropertiesQuery, ResourceTemplateDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    private readonly IStringLocalizer<SharedResource> _sharedLocalizer; // ADDED

    public GetTemplateWithPropertiesHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IStringLocalizer<ErrorMessages> localizer,
        IStringLocalizer<SharedResource> sharedLocalizer) // ADDED
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
        _sharedLocalizer = sharedLocalizer; // ADDED
    }

    public async Task<ResourceTemplateDto?> Handle(GetTemplateWithPropertiesQuery request, CancellationToken cancellationToken)
    {
        var template = await _unitOfWork.ResourceTemplates.GetTemplateWithPropertiesAsync(request.Id);
        if (template == null) return null;
        
        var dto = _mapper.Map<ResourceTemplateDto>(template);
        
        // CHANGED: Use _sharedLocalizer for "Error"
        if (dto == null) throw new Exception(_sharedLocalizer["Error"]); 
        
        return dto;
    }
}