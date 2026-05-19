using LMS.App.DTOs.Vocabulary;
using MediatR;

namespace LMS.App.Features.Vocabularies.Commands.CreateVocabulary;

public record CreateVocabularyCommand(
    CreateVocabularyDto Dto
) : IRequest<int>;