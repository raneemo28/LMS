using MediatR;
using LMS.Domain.Interfaces;
using AutoMapper;
using LMS.Domain.Constants;
namespace LMS.App.Features.ItemSets.Commands.UpdateItemSets;
 
public class UpdateItemSetHandler : IRequestHandler<UpdateItemSetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateItemSetHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateItemSetCommand request, CancellationToken cancellationToken)
    {
        var itemSet = await _unitOfWork.ItemSets.GetByIdAsync(request.Dto.Id);
        if (itemSet == null) return false;

        bool isAdmin = request.UserRoles.Contains(Roles.Admin);
        if (!isAdmin && itemSet.OwnerId != request.UserId)
            throw new UnauthorizedAccessException("You are not authorized to update this item set.");

         _mapper.Map(request.Dto, itemSet);
        itemSet.ModifiedAt = DateTime.UtcNow;
        itemSet.ModifiedBy = request.UserId;

        _unitOfWork.ItemSets.Update(itemSet);
        return await _unitOfWork.CommitAsync() > 0;
    }
}
