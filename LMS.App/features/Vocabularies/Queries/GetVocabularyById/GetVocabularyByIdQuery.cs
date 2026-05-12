using MediatR;
using LMS.App.DTOs.Vocabulary;
namespace LMS.App.Features.Vocabularies.Queries.GetVocabularyById;
public record GetVocabularyByIdQuery(int Id) : IRequest<VocabularyDto?>;
