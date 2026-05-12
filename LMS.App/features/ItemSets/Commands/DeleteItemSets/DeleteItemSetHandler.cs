using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.ItemSets.Commands.DeleteItemSets;

public class DeleteItemSetHandler : IRequestHandler<DeleteItemSetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteItemSetHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteItemSetCommand request, CancellationToken cancellationToken)
    {
        var itemSet = await _unitOfWork.ItemSets.GetByIdAsync(request.Id);
        
        if (itemSet == null) return false;

        bool isOwner = await _unitOfWork.ItemSets.IsOwnerAsync(request.Id, request.UserId);

        if (!isOwner)
        {
            throw new UnauthorizedAccessException("Do not have permission to delete this item set.");
        }

        _unitOfWork.ItemSets.Delete(itemSet);

        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}
