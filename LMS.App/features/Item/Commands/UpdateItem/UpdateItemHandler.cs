using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;

namespace LMS.Application.Features.Item.Commands.UpdateItem;

public class UpdateItemHandler : IRequestHandler<UpdateItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateItemHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var resource = await _unitOfWork.Items.GetByIdAsync(request.Id);
        var item = resource as Domain.Entities.Item;

        if (item == null) return false;

        if (item.OwnerId != request.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this item.");
        }

        item.TemplateId = request.TemplateId;

        
        item.ModifiedAt = DateTime.UtcNow;
        item.ModifiedBy = request.UserId;

        _unitOfWork.Items.Update(item);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}