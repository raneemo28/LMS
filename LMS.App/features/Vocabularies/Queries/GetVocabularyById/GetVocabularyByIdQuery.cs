using MediatR;
using LMS.App.DTOs.Vocabulary;
namespace LMS.Application.Features.Vocabularies.Queries.GetVocabularyById;
public record GetVocabularyByIdQuery(int Id) : IRequest<VocabularyDto?>;