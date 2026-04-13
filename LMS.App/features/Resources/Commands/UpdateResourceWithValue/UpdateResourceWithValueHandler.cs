using MediatR;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;

namespace LMS.Application.Features.Resources.Commands.UpdateResourceWithValue;

public class UpdateResourceWithValueHandler : IRequestHandler<UpdateResourceWithValueCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateResourceWithValueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateResourceWithValueCommand request, CancellationToken cancellationToken)
    {
        var resource = await _unitOfWork.Resources.GetByIdAsync(request.ResourceId);
        if (resource == null) return false;

        resource.ModifiedAt = DateTime.Now;
        resource.ModifiedBy = request.ModifiedBy;

        var updatedValue = new Value
        {
            Id = request.ValueId,
            ResourceId = request.ResourceId,
            ValueText = request.ValueText,
            ValueUri = request.ValueUri,
            ValueResourceId = request.ValueResourceId,
            Type = request.ValueType,
            Language = request.Language
        };

        var success = await _unitOfWork.Resources.UpdateResourceWithValue(request.ResourceId, updatedValue);

        if (!success) return false;

        _unitOfWork.Resources.Update(resource);

        return await _unitOfWork.CommitAsync() > 0;
    }
}