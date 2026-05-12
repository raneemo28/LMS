using MediatR;
namespace LMS.App.Features.Vocabularies.Commands.UpdateVocabulary;
public record UpdateVocabularyCommand(int Id, string Prefix, string NamespaceUri, string Label) : IRequest<bool>;
