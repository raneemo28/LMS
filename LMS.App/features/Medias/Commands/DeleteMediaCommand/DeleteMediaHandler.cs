using LMS.Domain.Interfaces;
using LMS.App.Interface;
using MediatR;

namespace LMS.App.Features.Media.Commands.DeleteMediaCommand;

public class DeleteMediaCommandHandler : IRequestHandler<DeleteMediaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaStorageService _storage;

    public DeleteMediaCommandHandler(IUnitOfWork unitOfWork, IMediaStorageService storage)
    {
        _unitOfWork = unitOfWork;
        _storage = storage;
    }

    public async Task<bool> Handle(DeleteMediaCommand request, CancellationToken cancellationToken)
    {
        var resource = await _unitOfWork.Media.GetByIdAsync(request.MediaId);
        var media = resource as Domain.Entities.Media;
        if (media == null) return false;

        if (media.OwnerId != request.CurrentUserId)
        {
            throw new UnauthorizedAccessException("You don't have permission to delete this file.");
        }

        if (!string.IsNullOrWhiteSpace(media.StoragePath))
        {
            await _storage.DeleteAsync(media.StoragePath);
        }

        _unitOfWork.Media.Delete(media);

        return await _unitOfWork.CommitAsync() > 0;
    }
}
