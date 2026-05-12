using MediatR;

namespace LMS.App.Features.Resources.Commands.RemoveValue;

public record RemoveValueCommand(
    int ResourceId,
    int ValueId
) : IRequest<bool>;
