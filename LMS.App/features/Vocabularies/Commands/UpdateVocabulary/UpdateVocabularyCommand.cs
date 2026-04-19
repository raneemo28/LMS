using MediatR;
namespace LMS.Application.Features.Vocabularies.Commands.UpdateVocabulary;
public record UpdateVocabularyCommand(int Id, string Prefix, string NamespaceUri, string Label) : IRequest<bool>;
