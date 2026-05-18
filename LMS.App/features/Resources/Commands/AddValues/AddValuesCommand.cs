using MediatR;
using LMS.App.DTOs.Value;
using System.Collections.Generic;

namespace LMS.App.Features.Resources.Commands.AddValues;

public record AddValuesCommand(
    int ResourceId,
    List<CreateResourceValueDto> Values
) : IRequest<bool>;
