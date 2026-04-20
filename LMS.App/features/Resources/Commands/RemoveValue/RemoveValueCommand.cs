using MediatR;

namespace LMS.Application.Features.Resources.Commands.RemoveValue;

public record RemoveValueCommand(
    int ResourceId,
    int ValueId
) : IRequest<bool>;
