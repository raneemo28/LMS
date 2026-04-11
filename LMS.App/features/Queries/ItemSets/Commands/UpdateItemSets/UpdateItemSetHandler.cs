using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

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
        

        if (resource is not ItemSet existingItemSet) 
            return false;

        bool isOwner = await _unitOfWork.ItemSets.IsOwnerAsync(request.Id, request.UserId);
        bool isAdmin = request.UserRoles?.Contains("Admin") ?? false;

        if (!isOwner && !isAdmin)
        {
            throw new UnauthorizedAccessException("Do not have permission to update this item set.");
        }

        existingItemSet.Title = request.Title;
        existingItemSet.Description = request.Description;
        existingItemSet.IsPublic = request.IsPublic;

        _unitOfWork.ItemSets.Update(existingItemSet);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}