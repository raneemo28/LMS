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
        var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(request.Id);

        if (vocabulary == null)
        {
            throw new Exception("The vocabulary with the specified ID does not exist.");
        }

        _unitOfWork.Vocabularies.Delete(vocabulary);

        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}