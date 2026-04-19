using MediatR;
using LMS.App.DTOs.Vocabulary;
namespace LMS.Application.Features.Vocabularies.Queries.GetAllVocabularies;
public record GetAllVocabulariesQuery : IRequest<List<VocabularyDto>>;