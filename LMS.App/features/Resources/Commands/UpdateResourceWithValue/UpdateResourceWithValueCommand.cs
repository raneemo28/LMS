using MediatR;

namespace LMS.Application.Features.Resources.Commands.UpdateResourceWithValue;

public record UpdateResourceWithValueCommand(
    int ResourceId,
    int ValueId,
    string? ValueText,
    string? ValueUri,
    int? ValueResourceId,
    string ValueType,
    string? Language,
    string? ModifiedBy 
) : IRequest<bool>;