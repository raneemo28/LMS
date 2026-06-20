using MediatR;
using LMS.Domain.Interfaces;
using LMS.App.Interface;
using LMS.App.DTOs.Media;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Medias.Commands.DownloadMedia;

public class DownloadMediaHandler
    : IRequestHandler<DownloadMediaCommand, DownloadMediaResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaStorageService _storage;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public DownloadMediaHandler(
        IUnitOfWork unitOfWork,
        IMediaStorageService storage,
        IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _storage = storage;
        _localizer = localizer;
    }

    public async Task<DownloadMediaResult> Handle(
        DownloadMediaCommand request,
        CancellationToken cancellationToken)
    {
        var resource = await _unitOfWork.Media.GetByIdAsync(request.MediaId);

        var media = resource as Domain.Entities.Media;

        if (media == null)
            throw new KeyNotFoundException(_localizer["MediaNotFound"]);

        if (string.IsNullOrWhiteSpace(media.StoragePath))
            throw new InvalidOperationException(_localizer["MediaStoragePathNotSet"]);

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
