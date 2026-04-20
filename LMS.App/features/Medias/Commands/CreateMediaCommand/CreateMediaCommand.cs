using MediatR;
using LMS.Domain.Constants;
using LMS.App.DTOs.Value;

namespace LMS.App.Features.Media.Commands.CreateMediaCommand;
public record CreateMediaCommand(
    int? ItemId,
    string FileName,
    string? AltText,
    string? OwnerId,
    List<ResourceValueDto> Values
) : IRequest<int>;