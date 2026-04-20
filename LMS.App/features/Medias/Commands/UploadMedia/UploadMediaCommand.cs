using MediatR;

namespace LMS.App.Features.Media.Commands.UploadMediaFile;

public record UploadMediaFileCommand(
    int MediaId,
    byte[] Content,
    string MimeType,
    string FileName
) : IRequest<string>;