using MediatR;
namespace LMS.App.Features.Vocabularies.Commands.CreateProperty;
public record CreatePropertyCommand(int VocabularyId, string LocalName, string Label, string TermUri) : IRequest<int>;
