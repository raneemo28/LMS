using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Vocabularies.Commands.CreateVocabulary;

public class CreateVocabularyHandler : IRequestHandler<CreateVocabularyCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateVocabularyHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateVocabularyCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Vocabularies.IsLabelUniqueAsync(request.Label))
        {
            throw new Exception("The label is already in use.");
        }

        if (!await _unitOfWork.Vocabularies.IsNamespaceUriUniqueAsync(request.NamespaceUri))
        {
            throw new Exception("The Namespace URI must be unique.");
        }

        var vocabulary = new LMS.Domain.Entities.Vocabulary
        {
            Prefix = request.Prefix,
            NamespaceUri = request.NamespaceUri,
            Label = request.Label
        };

        await _unitOfWork.Vocabularies.AddAsync(vocabulary);
        await _unitOfWork.CommitAsync();

        return vocabulary.Id;
    }
}