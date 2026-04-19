using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Vocabularies.Commands.UpdateVocabulary;

public class UpdateVocabularyHandler : IRequestHandler<UpdateVocabularyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateVocabularyHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(UpdateVocabularyCommand request, CancellationToken ct)
    {
        var vocab = await _unitOfWork.Vocabularies.GetByIdAsync(request.Id);
        if (vocab == null) return false;

        if (vocab.Label != request.Label && !await _unitOfWork.Vocabularies.IsLabelUniqueAsync(request.Label))
            throw new InvalidOperationException($"Vocabulary label '{request.Label}' is already in use.");

        vocab.Prefix = request.Prefix;
        vocab.NamespaceUri = request.NamespaceUri;
        vocab.Label = request.Label;

        _unitOfWork.Vocabularies.Update(vocab);
        return await _unitOfWork.CommitAsync() > 0;
    }
}