using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Vocabularies.Commands.DeleteProperty;

public class DeletePropertyHandler : IRequestHandler<DeletePropertyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeletePropertyHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeletePropertyCommand request, CancellationToken ct)
    {
        if (await _unitOfWork.Vocabularies.HasLinkedValuesAsync(request.Id))
            throw new InvalidOperationException("Cannot delete property because it has linked values.");

        var deleted = await _unitOfWork.Vocabularies.DeletePropertyAsync(request.Id);
        return deleted && await _unitOfWork.CommitAsync() > 0;
    }
}
