using MediatR;
using LMS.Domain.Interfaces;

namespace LMS.App.Features.Vocabularies.Commands.DeleteProperty;

public class DeletePropertyHandler : IRequestHandler<DeletePropertyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePropertyHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
    {
        var hasValues = await _unitOfWork.Vocabularies.HasLinkedValuesAsync(request.PropertyId);

        if (hasValues)
        {
            throw new InvalidOperationException("Cannot delete property because it has linked values. Please remove the linked values first.");
        }

        var deletedProperty = await _unitOfWork.Vocabularies.DeletePropertyAync(request.PropertyId);

        if (deletedProperty == null) return false;

        return await _unitOfWork.CommitAsync() > 0;
    }
}