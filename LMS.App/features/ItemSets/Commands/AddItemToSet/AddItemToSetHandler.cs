using MediatR;
using LMS.Domain.Interfaces;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.ItemSets.Commands.AddItemToSet;

public class AddItemToSetHandler : IRequestHandler<AddItemToSetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public AddItemToSetHandler(IUnitOfWork unitOfWork, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<bool> Handle(AddItemToSetCommand request, CancellationToken cancellationToken)
    {
        var itemSet = await _unitOfWork.ItemSets.GetByIdAsync(request.SetId);
        if (itemSet == null) return false;

        if (itemSet.OwnerId != request.UserId)
            throw new UnauthorizedAccessException(_localizer["NoPermissionModifySet"]);

        var result = await _unitOfWork.ItemSets.AddItemToSetAsync(request.SetId, request.ItemId);
        if (result == null) return false;

        itemSet.ModifiedAt = DateTime.UtcNow;
        itemSet.ModifiedBy = request.UserId;
        return await _unitOfWork.CommitAsync() > 0;
    }
}