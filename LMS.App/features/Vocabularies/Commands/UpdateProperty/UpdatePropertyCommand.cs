using MediatR;

namespace LMS.Application.Features.Vocabularies.Commands.UpdateProperty;

public record UpdatePropertyCommand(
    int PropertyId, 
    string LocalName, 
    string Label, 
    string TermUri
) : IRequest<bool>;