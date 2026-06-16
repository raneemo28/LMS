using MediatR;
using LMS.Domain.Interfaces;
using LMS.App.Interface;
using LMS.Domain.Entities;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Media.Commands.UploadMediaFile;

public class UploadMediaFileHandler : IRequestHandler<UploadMediaFileCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaStorageService _storage;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public UploadMediaFileHandler(IUnitOfWork unitOfWork, IMediaStorageService storage, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _storage = storage;
        _localizer = localizer;
    }

    public async Task<string> Handle(UploadMediaFileCommand request, CancellationToken cancellationToken)
    {
        if (request.Content is null || request.Content.Length == 0)
            throw new ArgumentException(_localizer["FileContentEmpty"]);

        var media = await _unitOfWork.Media.GetByIdAsync(request.MediaId);
        if (media is null)
            throw new KeyNotFoundException(_localizer["MediaNotFound"]);

        var storagePath = await _storage.UploadAsync(request.Content, request.FileName, request.MimeType);
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