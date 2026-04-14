using MediatR;

namespace LMS.App.Features.Medias.Commands.UpdateMediaCommand;

public record UpdateMediaCommand(
    int Id,
    string? AltText,
    string FileName,
    string StoragePath,
    string? MimeType,
    long? FileSize,
    int? ItemId,
    string? CreatedBy,
    string CurrentUserId
) : IRequest<bool>;