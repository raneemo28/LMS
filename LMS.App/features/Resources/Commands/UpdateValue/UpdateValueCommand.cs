using MediatR;

namespace LMS.Application.Features.Resources.Commands.UpdateValue;

public record UpdateValueCommand(
    int ResourceId,
    int ValueId,
    string? ValueText,
    string? ValueUri,
    int? ValueResourceId,
    string Type,
    string? Language
) : IRequest<bool>;
