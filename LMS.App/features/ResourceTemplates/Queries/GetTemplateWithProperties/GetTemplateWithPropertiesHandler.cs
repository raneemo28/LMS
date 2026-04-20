using LMS.Domain.Interfaces;
using MediatR;
using AutoMapper;
using LMS.App.DTOs.ResourceTemplate;

namespace LMS.Application.Features.ResourceTemplates.Queries.GetTemplateWithProperties;

public class GetTemplateWithPropertiesHandler : IRequestHandler<GetTemplateWithPropertiesQuery, ResourceTemplateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTemplateWithPropertiesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResourceTemplateDto> Handle(GetTemplateWithPropertiesQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ResourceTemplates.GetTemplateWithPropertiesAsync(request.Id);

        if (result == null)
            throw new KeyNotFoundException($"Template with ID {request.Id} was not found.");

        return _mapper.Map<ResourceTemplateDto>(result);
    }
}