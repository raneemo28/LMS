using MediatR;

namespace LMS.App.Features.Resources.Commands.UpdateValue;

public record UpdateValueCommand(
    int ResourceId,
    int ValueId,
    string? ValueText,
    int? ValueResourceId,
    string Type,
    string? Language
) : IRequest<bool>;
