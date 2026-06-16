using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Items.Commands.DeleteItem;

public class DeleteItemHandler : IRequestHandler<DeleteItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public DeleteItemHandler(IUnitOfWork unitOfWork, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(request.Id);
        if (item == null) return false;

        if (item.OwnerId != request.UserId)
            throw new UnauthorizedAccessException(_localizer["NoPermissionDeleteItem"]);

        _unitOfWork.Items.Delete(item);
        return await _unitOfWork.CommitAsync() > 0;
    }
}