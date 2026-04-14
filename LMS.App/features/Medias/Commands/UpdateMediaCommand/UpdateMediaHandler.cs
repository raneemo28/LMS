using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;

namespace LMS.App.Features.Medias.Commands.UpdateMediaCommand;

public class UpdateMediaCommandHandler : IRequestHandler<UpdateMediaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMediaCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateMediaCommand request, CancellationToken cancellationToken)
    {

        var resource = await _unitOfWork.Media.GetByIdAsync(request.Id);
        var media = resource as Domain.Entities.Media;
        if (media == null) return false;


        if (media.OwnerId != request.CurrentUserId)
        {
            throw new UnauthorizedAccessException("Don't have permission to edit this file.");
        }

        media.FileName = request.FileName;
        media.StoragePath = request.StoragePath;
        media.MimeType = request.MimeType;
        media.FileSize = request.FileSize;
        media.AltText = request.AltText;
        media.ItemId = request.ItemId;

        media.ModifiedAt = DateTime.UtcNow;
        media.ModifiedBy = request.CurrentUserId;

        _unitOfWork.Media.Update(media);
        // _unitOfWork.Resource.Update(resource);
        return await _unitOfWork.CommitAsync() > 0;
    }
}