using MediatR;
using LMS.Domain.Interfaces;
using AutoMapper;
using LMS.Domain.Constants;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.ItemSets.Commands.UpdateItemSets;

public class UpdateItemSetHandler : IRequestHandler<UpdateItemSetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public UpdateItemSetHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<bool> Handle(UpdateItemSetCommand request, CancellationToken cancellationToken)
    {
        var itemSet = await _unitOfWork.ItemSets.GetByIdAsync(request.Dto.Id);
        if (itemSet == null) return false;

        bool isAdmin = request.UserRoles.Contains(Roles.Admin);
        if (!isAdmin && itemSet.OwnerId != request.UserId)
            throw new UnauthorizedAccessException(_localizer["NotAuthorizedUpdateItemSet"]);

        _mapper.Map(request.Dto, itemSet);
        itemSet.ModifiedAt = DateTime.UtcNow;
        itemSet.ModifiedBy = request.UserId;
        _unitOfWork.ItemSets.Update(itemSet);
        return await _unitOfWork.CommitAsync() > 0;
    }
}