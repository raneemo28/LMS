using AutoMapper;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Vocabularies.Commands.UpdateVocabulary;

public class UpdateVocabularyHandler : IRequestHandler<UpdateVocabularyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateVocabularyHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateVocabularyCommand request, CancellationToken ct)
    {
        var vocab = await _unitOfWork.Vocabularies.GetByIdAsync(request.Id);
        if (vocab == null) return false;

        if (vocab.Label != request.Dto.Label &&
            !await _unitOfWork.Vocabularies.IsLabelUniqueAsync(request.Dto.Label))
            throw new InvalidOperationException($"Vocabulary label '{request.Dto.Label}' is already in use.");

        if (vocab.NamespaceUri != request.Dto.NamespaceUri &&
            !await _unitOfWork.Vocabularies.IsNamespaceUriUniqueAsync(request.Dto.NamespaceUri))
            throw new InvalidOperationException($"Vocabulary namespace URI '{request.Dto.NamespaceUri}' is already in use.");

        // ✅ Mapper updates the existing tracked entity in-place
        _mapper.Map(request.Dto, vocab);

        _unitOfWork.Vocabularies.Update(vocab);
        return await _unitOfWork.CommitAsync() > 0;
    }
}