using LMS.App.DTOs.Property;
using MediatR;

namespace LMS.App.Features.Vocabularies.Commands.UpdateProperty;

public record UpdatePropertyCommand(
    int Id,
    UpdatePropertyDto Dto
) : IRequest<bool>;