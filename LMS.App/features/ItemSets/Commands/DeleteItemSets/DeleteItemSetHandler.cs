using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.ItemSets.Commands.DeleteItemSets;

public class DeleteItemSetHandler : IRequestHandler<DeleteItemSetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public DeleteItemSetHandler(IUnitOfWork unitOfWork, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<bool> Handle(DeleteItemSetCommand request, CancellationToken cancellationToken)
    {
        var itemSet = await _unitOfWork.ItemSets.GetByIdAsync(request.Id);
        if (itemSet == null) return false;

        bool isOwner = await _unitOfWork.ItemSets.IsOwnerAsync(request.Id, request.UserId);
        if (!isOwner)
            throw new UnauthorizedAccessException(_localizer["NoPermissionDeleteSet"]);

        _unitOfWork.ItemSets.Delete(itemSet);
        return await _unitOfWork.CommitAsync() > 0;
    }
}