using AutoMapper;
using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Vocabularies.Commands.UpdateVocabulary;

public class UpdateVocabularyHandler : IRequestHandler<UpdateVocabularyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public UpdateVocabularyHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<bool> Handle(UpdateVocabularyCommand request, CancellationToken ct)
    {
        var vocab = await _unitOfWork.Vocabularies.GetByIdAsync(request.Id);
        if (vocab == null) return false;

        if (vocab.Label != request.Dto.Label && !await _unitOfWork.Vocabularies.IsLabelUniqueAsync(request.Dto.Label))
            throw new InvalidOperationException(_localizer["LabelAlreadyExists"]);

        if (vocab.NamespaceUri != request.Dto.NamespaceUri && !await _unitOfWork.Vocabularies.IsNamespaceUriUniqueAsync(request.Dto.NamespaceUri))
            throw new InvalidOperationException(_localizer["LabelAlreadyExists"]);

        _mapper.Map(request.Dto, vocab);
        _unitOfWork.Vocabularies.Update(vocab);
        return await _unitOfWork.CommitAsync() > 0;
    }
}