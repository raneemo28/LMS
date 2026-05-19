using AutoMapper;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Vocabularies.Commands.CreateVocabulary;

public class CreateVocabularyHandler : IRequestHandler<CreateVocabularyCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateVocabularyHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateVocabularyCommand request, CancellationToken ct)
    {
        if (!await _unitOfWork.Vocabularies.IsLabelUniqueAsync(request.Dto.Label))
            throw new InvalidOperationException($"Vocabulary label '{request.Dto.Label}' is already in use.");

        if (!await _unitOfWork.Vocabularies.IsNamespaceUriUniqueAsync(request.Dto.NamespaceUri))
            throw new InvalidOperationException($"Namespace URI '{request.Dto.NamespaceUri}' is already in use.");

        // ✅ Mapper does the construction — VocabularyMappingProfile: CreateVocabularyDto → Vocabulary
        var vocabulary = _mapper.Map<Vocabulary>(request.Dto);

        await _unitOfWork.Vocabularies.AddAsync(vocabulary);
        await _unitOfWork.CommitAsync();

        return vocabulary.Id;
    }
}