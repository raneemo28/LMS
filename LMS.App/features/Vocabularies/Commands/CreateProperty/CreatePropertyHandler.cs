using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Vocabularies.Commands.CreateProperty;

public class CreatePropertyHandler : IRequestHandler<CreatePropertyCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreatePropertyHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<int> Handle(CreatePropertyCommand request, CancellationToken ct)
    {
        var vocab = await _unitOfWork.Vocabularies.GetByIdAsync(request.VocabularyId);
        if (vocab == null) throw new KeyNotFoundException($"Vocabulary ID {request.VocabularyId} not found.");

        var property = new Property
        {
            VocabularyId = request.VocabularyId,
            LocalName = request.LocalName,
            Label = request.Label,
            TermUri = request.TermUri
        };

        await _unitOfWork.Vocabularies.AddPropertyAsync(property);
        await _unitOfWork.CommitAsync();
        return property.Id;
    }
}