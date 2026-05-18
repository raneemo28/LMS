using MediatR;

namespace LMS.App.Features.Media.Commands.UploadMediaFile;

public record UploadMediaFileCommand(
    int MediaId,
    System.IO.Stream Content,
    string MimeType,
    long FileSize,
    string FileName
) : IRequest<string>;
