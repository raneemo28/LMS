using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.ResourceTemplates.Queries.GetTemplateWithProperties;

public class GetTemplateWithPropertiesHandler : IRequestHandler<GetTemplateWithPropertiesQuery, object>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTemplateWithPropertiesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<object> Handle(GetTemplateWithPropertiesQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ResourceTemplates.GetTemplateWithPropertiesAsync(request.Id);

        if (result == null)
            throw new KeyNotFoundException($"Template with ID {request.Id} was not found.");

        return result;
    }
}