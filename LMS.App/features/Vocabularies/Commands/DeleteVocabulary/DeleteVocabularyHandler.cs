using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Vocabularies.Commands.DeleteVocabulary;

public class DeleteVocabularyHandler : IRequestHandler<DeleteVocabularyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public DeleteVocabularyHandler(IUnitOfWork unitOfWork, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<bool> Handle(DeleteVocabularyCommand request, CancellationToken ct)
    {
        var vocab = await _unitOfWork.Vocabularies.GetByIdAsync(request.Id);
        if (vocab == null) return false;

        var props = await _unitOfWork.Vocabularies.GetPropertiesByVocabularyIdAsync(request.Id);
        foreach (var prop in props)
        {
            if (await _unitOfWork.Vocabularies.HasLinkedValuesAsync(prop.Id))
                throw new InvalidOperationException(_localizer["CannotDeleteLinkedProperty"]);
        }

        _unitOfWork.Vocabularies.Delete(vocab);
        return await _unitOfWork.CommitAsync() > 0;
    }
}