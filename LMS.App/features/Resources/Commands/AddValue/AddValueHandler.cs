using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Resources.Commands.AddValue;

public class AddValueHandler : IRequestHandler<AddValueCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddValueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AddValueCommand request, CancellationToken cancellationToken)
    {
        var property = await _unitOfWork.Vocabularies.GetPropertyByIdAsync(request.PropertyId);
        if (property == null)
            throw new KeyNotFoundException($"Property {request.PropertyId} not found.");

        // System constructs the URI from the property definition — not user-supplied.
        var systemUri = property.TermUri;

        var value = await _unitOfWork.Resources.AddValueAsync(
            request.ResourceId,
            request.PropertyId,
            request.ValueText,
            systemUri,
            request.ValueResourceId,
            request.Type,
            request.Language
        );
        if (value == null) return false;
        return await _unitOfWork.CommitAsync() > 0;
    }
}
