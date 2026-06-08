using LMS.App.DTOs.Property;
using MediatR;

namespace LMS.App.Features.Vocabularies.Commands.CreateProperty;

public record CreatePropertyCommand(
    int VocabularyId,
    CreatePropertyDto Dto
) : IRequest<int>;