using MediatR;
using LMS.App.DTOs.Vocabulary;
namespace LMS.App.Features.Vocabularies.Queries.GetAllVocabularies;
public record GetAllVocabulariesQuery : IRequest<List<VocabularyDto>>;
