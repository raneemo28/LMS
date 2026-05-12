using LMS.App.DTOs.Value;
using MediatR;

namespace LMS.App.Features.Medias.Commands.UpdateMediaCommand;

public record UpdateMediaCommand(
    int Id,
    int? ItemId,
    string FileName,
    string? AltText,
    List<ResourceValueDto> Values, // القائمة الجديدة للقيم
    string CurrentUserId
) : IRequest<bool>;
