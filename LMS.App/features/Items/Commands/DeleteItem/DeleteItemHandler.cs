using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Items.Commands.DeleteItem;

public class DeleteItemHandler : IRequestHandler<DeleteItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteItemHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(request.Id);
        
        if (item == null) return false;

        if (item.OwnerId != request.UserId)
        {
            throw new UnauthorizedAccessException("Don't have permission to delete this item.");
        }

        _unitOfWork.Items.Delete(item);

        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}
