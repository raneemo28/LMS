using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;

namespace LMS.Application.Features.ItemSets.Commands.UpdateItemSets;

public class UpdateItemSetHandler : IRequestHandler<UpdateItemSetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateItemSetHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateItemSetCommand request, CancellationToken cancellationToken)
    {
        var resource = await _unitOfWork.ItemSets.GetByIdAsync(request.Id);
        var itemSet = resource as ItemSet;
        if (itemSet == null) return false;

        bool isOwner = itemSet.OwnerId == request.UserId;

        if (!isOwner)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this item set.");
        }

        itemSet.Title = request.Title;
        itemSet.Description = request.Description;
        itemSet.IsPublic = request.IsPublic;

        itemSet.ModifiedAt = DateTime.UtcNow;
        itemSet.ModifiedBy = request.UserId;

        _unitOfWork.ItemSets.Update(itemSet);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}