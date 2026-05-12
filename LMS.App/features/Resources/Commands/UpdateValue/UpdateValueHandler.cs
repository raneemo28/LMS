using LMS.Domain.Interfaces;
using MediatR;
using System.Linq;

namespace LMS.App.Features.Resources.Commands.UpdateValue;

public class UpdateValueHandler : IRequestHandler<UpdateValueCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateValueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateValueCommand request, CancellationToken cancellationToken)
    {
        // Fetch the existing value to know its PropertyId
        // (ResourceRepository.UpdateValueAsync already fetches the value by valueId+resourceId)
        // We need to look up the property to reconstruct the URI.
        var values = await _unitOfWork.Resources.GetResourceValuesAsync(request.ResourceId);
        var existing = values.FirstOrDefault(v => v.Id == request.ValueId);
        if (existing == null) return false;

        var property = await _unitOfWork.Vocabularies.GetPropertyByIdAsync(existing.PropertyId);
        var systemUri = property?.TermUri;

        var result = await _unitOfWork.Resources.UpdateValueAsync(
            request.ResourceId,
            request.ValueId,
            request.ValueText,
            systemUri,
            request.ValueResourceId,
            request.Type,
            request.Language
        );
        if (result) await _unitOfWork.CommitAsync();
        return result;
    }
}
