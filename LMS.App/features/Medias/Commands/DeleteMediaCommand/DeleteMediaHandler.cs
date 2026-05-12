using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Media.Commands.DeleteMediaCommand;

public class DeleteMediaCommandHandler : IRequestHandler<DeleteMediaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMediaCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

        if (File.Exists(media.StoragePath))
        {
            File.Delete(media.StoragePath);
        }

        _unitOfWork.Media.Delete(media);

        return await _unitOfWork.CommitAsync() > 0;
    }
}
