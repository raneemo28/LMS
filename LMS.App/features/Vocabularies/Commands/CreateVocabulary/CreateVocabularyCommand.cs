using MediatR;
namespace LMS.App.Features.Vocabularies.Commands.CreateVocabulary;
public record CreateVocabularyCommand(string Prefix, string NamespaceUri, string Label) : IRequest<int>;
