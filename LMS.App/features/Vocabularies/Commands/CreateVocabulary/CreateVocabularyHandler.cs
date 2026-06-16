using AutoMapper;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Vocabularies.Commands.CreateVocabulary;

public class CreateVocabularyHandler : IRequestHandler<CreateVocabularyCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public CreateVocabularyHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateVocabularyCommand request, CancellationToken ct)
    {
        if (!await _unitOfWork.Vocabularies.IsLabelUniqueAsync(request.Dto.Label))
            throw new InvalidOperationException(_localizer["LabelAlreadyExists"]);

        if (!await _unitOfWork.Vocabularies.IsNamespaceUriUniqueAsync(request.Dto.NamespaceUri))
            throw new InvalidOperationException(_localizer["LabelAlreadyExists"]);

        var vocabulary = _mapper.Map<Vocabulary>(request.Dto);
        await _unitOfWork.Vocabularies.AddAsync(vocabulary);
        await _unitOfWork.CommitAsync();
        return vocabulary.Id;
    }
}