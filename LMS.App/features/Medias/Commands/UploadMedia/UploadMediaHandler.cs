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
        if (request.Content is null || request.Content.Length == 0)
            throw new ArgumentException("File content cannot be empty.");

        var media = await _unitOfWork.Media.GetByIdAsync(request.MediaId);
        if (media is null)
            throw new KeyNotFoundException($"Media with ID {request.MediaId} not found.");

        var storagePath = await _storage.UploadAsync(
            request.Content,
            request.FileName,
            request.MimeType);

        try
        {
            media.ModifiedAt = DateTime.UtcNow;
            media.StoragePath = storagePath;
            media.MimeType = request.MimeType;
            media.FileName = request.FileName;

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _storage.DeleteAsync(storagePath);
            throw;
        }

        return storagePath;
    }
}
