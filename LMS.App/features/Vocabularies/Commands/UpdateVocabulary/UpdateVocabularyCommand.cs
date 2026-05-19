using LMS.App.DTOs.Vocabulary;
using MediatR;

namespace LMS.App.Features.Vocabularies.Commands.UpdateVocabulary;

public record UpdateVocabularyCommand(
    int Id,
    UpdateVocabularyDto Dto
) : IRequest<bool>;