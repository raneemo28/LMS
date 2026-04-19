using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Vocabularies.Commands.CreateVocabulary;

public class CreateVocabularyHandler : IRequestHandler<CreateVocabularyCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateVocabularyHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<int> Handle(CreateVocabularyCommand request, CancellationToken ct)
    {
        if (!await _unitOfWork.Vocabularies.IsLabelUniqueAsync(request.Label))
            throw new InvalidOperationException($"Vocabulary label '{request.Label}' is already in use.");
        if (!await _unitOfWork.Vocabularies.IsNamespaceUriUniqueAsync(request.NamespaceUri))
            throw new InvalidOperationException($"Namespace URI '{request.NamespaceUri}' is already in use.");

        var vocabulary = new Vocabulary
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