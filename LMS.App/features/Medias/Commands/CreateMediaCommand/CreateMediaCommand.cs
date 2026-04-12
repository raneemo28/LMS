using MediatR;
using LMS.Domain.Constants;

namespace LMS.App.Features.Media.Commands.CreateMediaCommand;

public record CreateMediaCommand(
    string FileName,
    string StoragePath,
    string? MimeType,
    long? FileSize,
    int? ItemId,
    string? CreatedBy,
    string? AltText,
    string OwnerId
) : IRequest<int>;