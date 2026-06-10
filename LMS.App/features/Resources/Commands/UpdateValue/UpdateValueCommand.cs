using LMS.App.DTOs.Value;
using MediatR;

namespace LMS.App.Features.Resources.Commands.UpdateValue;

public record UpdateValueCommand(
    int ResourceId,
    int ValueId,
    UpdateResourceValueDto Dto
) : IRequest<bool>;