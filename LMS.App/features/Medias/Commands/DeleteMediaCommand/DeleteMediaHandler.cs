using LMS.Domain.Interfaces;
using LMS.App.Interface;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Media.Commands.DeleteMediaCommand;

public class DeleteMediaCommandHandler : IRequestHandler<DeleteMediaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaStorageService _storage;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public DeleteMediaCommandHandler(IUnitOfWork unitOfWork, IMediaStorageService storage, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _storage = storage;
        _localizer = localizer;
    }

    public async Task<bool> Handle(DeleteMediaCommand request, CancellationToken cancellationToken)
    {
        var resource = await _unitOfWork.Media.GetByIdAsync(request.MediaId);
        var media = resource as Domain.Entities.Media;
        if (media == null) return false;

        if (media.OwnerId != request.CurrentUserId && !request.IsAdmin)
            throw new UnauthorizedAccessException(_localizer["NoPermissionDeleteFile"]);

        if (!string.IsNullOrWhiteSpace(media.StoragePath))
            await _storage.DeleteAsync(media.StoragePath);

        _unitOfWork.Media.Delete(media);
        return await _unitOfWork.CommitAsync() > 0;
    }
}