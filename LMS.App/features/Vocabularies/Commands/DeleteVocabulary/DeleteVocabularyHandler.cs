using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Application.Features.Vocabularies.Commands.DeleteVocabulary;

public class DeleteVocabularyHandler : IRequestHandler<DeleteVocabularyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVocabularyHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteVocabularyCommand request, CancellationToken cancellationToken)
{
    var result = await _unitOfWork.Vocabularies.GetWithPropertiesAsync(request.Id);
    
    var vocab = result as Vocabulary;

    if (vocab == null) return false;

    foreach (var property in vocab.Properties) 
    {
        if (await _unitOfWork.Vocabularies.HasLinkedValuesAsync(property.Id))
        {
            throw new InvalidOperationException($"property with id {property.Id} has linked values. Please remove the linked values before deleting the vocabulary.");
        }
    }

    _unitOfWork.Vocabularies.Delete(vocab);
    return await _unitOfWork.CommitAsync() > 0;
}
}