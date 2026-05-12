using MediatR;
using LMS.Domain.Interfaces;
using LMS.App.Interface;
using LMS.Domain.Entities;

namespace LMS.App.Features.Media.Commands.UploadMediaFile;

public class UploadMediaFileHandler : IRequestHandler<UploadMediaFileCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaStorageService _storage;

    public UploadMediaFileHandler(
        IUnitOfWork unitOfWork,
        IMediaStorageService storage)
    {
        _unitOfWork = unitOfWork;
        _storage = storage;
    }

    public async Task<string> Handle(UploadMediaFileCommand request, CancellationToken cancellationToken)
    {
        var media = await _unitOfWork.Media.GetByIdAsync(request.MediaId) as Domain.Entities.Media;

        if (media is null)
            throw new Exception("Media not found");


        var storagePath = await _storage.UploadAsync(
            request.Content,
            request.FileName,
            request.MimeType);
        media.ModifiedAt = DateTime.UtcNow;
        media.StoragePath = storagePath;
        media.MimeType = request.MimeType;
        media.FileName = request.FileName;

        await _unitOfWork.CommitAsync();

        return storagePath;
    }
}
