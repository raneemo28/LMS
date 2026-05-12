using MediatR;
namespace LMS.App.Features.Vocabularies.Commands.UpdateProperty;
public record UpdatePropertyCommand(int Id, string LocalName, string Label, string TermUri) : IRequest<bool>;
