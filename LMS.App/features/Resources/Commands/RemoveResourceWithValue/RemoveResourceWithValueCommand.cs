using MediatR;

namespace LMS.Application.Features.Resources.Commands.RemoveResourceWithValue;

public record RemoveResourceWithValueCommand(
    int ResourceId, 
    int ValueId
) : IRequest<bool>;