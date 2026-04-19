using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Vocabularies.Commands.DeleteVocabulary;

public class DeleteVocabularyHandler : IRequestHandler<DeleteVocabularyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteVocabularyHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteVocabularyCommand request, CancellationToken ct)
    {
        var vocab = await _unitOfWork.Vocabularies.GetByIdAsync(request.Id);
        if (vocab == null) return false;

        var props = await _unitOfWork.Vocabularies.GetPropertiesByVocabularyIdAsync(request.Id);
        foreach (var prop in props)
        {
            if (await _unitOfWork.Vocabularies.HasLinkedValuesAsync(prop.Id))
                throw new InvalidOperationException($"Cannot delete: Property '{prop.LocalName}' has linked values. Remove them first.");
        }

        _unitOfWork.Vocabularies.Delete(vocab);
        return await _unitOfWork.CommitAsync() > 0;
    }
}