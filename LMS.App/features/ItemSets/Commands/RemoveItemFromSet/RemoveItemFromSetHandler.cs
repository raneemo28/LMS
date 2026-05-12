using MediatR;
using LMS.Domain.Interfaces;
namespace LMS.App.Features.ItemSets.Commands.RemoveItemFromSet;
public class RemoveItemFromSetHandler : IRequestHandler<RemoveItemFromSetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveItemFromSetHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(RemoveItemFromSetCommand request, CancellationToken cancellationToken)
    {
        var itemSet = await _unitOfWork.ItemSets.GetByIdAsync(request.SetId);
        if (itemSet == null) return false;

        if (itemSet.OwnerId != request.UserId)
            throw new UnauthorizedAccessException("Don't have permission to modify this set.");

        var result = await _unitOfWork.ItemSets.RemoveItemFromSetAsync(request.SetId, request.ItemId);

        if (result == null) return false;

        itemSet.ModifiedAt = DateTime.UtcNow;
        itemSet.ModifiedBy = request.UserId;

        return await _unitOfWork.CommitAsync() > 0;
    }
}
