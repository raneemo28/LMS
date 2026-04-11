using MediatR;

namespace LMS.Application.Features.Vocabularies.Commands.AddProperty;

// نرجع الكيان Property نفسه بعد الإضافة
public record AddPropertyCommand(
    int VocabularyId, 
    string LocalName, 
    string Label, 
    string TermUri
) : IRequest<LMS.Domain.Entities.Property>;