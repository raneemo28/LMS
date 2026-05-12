using MediatR;
using LMS.Domain.Interfaces;
using LMS.App.Interface;
using LMS.App.DTOs.Media;

namespace LMS.App.Features.Medias.Commands.DownloadMedia;

public class DownloadMediaHandler
    : IRequestHandler<DownloadMediaCommand, DownloadMediaResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaStorageService _storage;

    public DownloadMediaHandler(
        IUnitOfWork unitOfWork,
        IMediaStorageService storage)
    {
        _unitOfWork = unitOfWork;
        _storage = storage;
    }

    public async Task<DownloadMediaResult> Handle(
        DownloadMediaCommand request,
        CancellationToken cancellationToken)
    {
        var resource = await _unitOfWork.Media.GetByIdAsync(request.MediaId);

        var media = resource as Domain.Entities.Media;

        if (media == null)
            throw new Exception("Media not found");

        if (string.IsNullOrWhiteSpace(media.StoragePath))
            throw new Exception("Invalid media storage path");

        (Stream stream, string contentType, string fileName) =
    await _storage.DownloadAsync(media.StoragePath);

        return new DownloadMediaResult
        {
            Stream = stream,
            ContentType = contentType,
            FileName = fileName
        };
    }
}
