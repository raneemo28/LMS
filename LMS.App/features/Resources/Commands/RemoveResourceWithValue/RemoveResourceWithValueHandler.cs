using MediatR;
using LMS.Domain.Interfaces;

namespace LMS.Application.Features.Resources.Commands.RemoveResourceWithValue;

public class RemoveResourceWithValueHandler : IRequestHandler<RemoveResourceWithValueCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveResourceWithValueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RemoveResourceWithValueCommand request, CancellationToken cancellationToken)
    {
        var resource = await _unitOfWork.Resources.GetByIdAsync(request.ResourceId);
        if (resource == null) return false;

        var success = await _unitOfWork.Resources.RemoveResourceWithValue(request.ResourceId, request.ValueId);

        if (!success) return false;

        resource.ModifiedAt = DateTime.Now;

        _unitOfWork.Resources.Update(resource);

        return await _unitOfWork.CommitAsync() > 0;
    }
}