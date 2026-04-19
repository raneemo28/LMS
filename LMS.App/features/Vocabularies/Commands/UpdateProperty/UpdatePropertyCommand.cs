using MediatR;
namespace LMS.Application.Features.Vocabularies.Commands.UpdateProperty;
public record UpdatePropertyCommand(int Id, string LocalName, string Label, string TermUri) : IRequest<bool>;
