using MediatR;
namespace LMS.Application.Features.Vocabularies.Commands.DeleteVocabulary;
public record DeleteVocabularyCommand(int Id) : IRequest<bool>;