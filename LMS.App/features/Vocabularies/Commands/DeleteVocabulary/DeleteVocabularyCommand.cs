using MediatR;
namespace LMS.App.Features.Vocabularies.Commands.DeleteVocabulary;
public record DeleteVocabularyCommand(int Id) : IRequest<bool>;
