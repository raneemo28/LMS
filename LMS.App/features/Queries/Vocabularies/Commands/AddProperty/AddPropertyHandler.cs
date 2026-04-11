using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Vocabularies.Commands.AddProperty;

public class AddPropertyHandler : IRequestHandler<AddPropertyCommand, LMS.Domain.Entities.Property>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddPropertyHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LMS.Domain.Entities.Property> Handle(AddPropertyCommand request, CancellationToken cancellationToken)
    {
        var property = await _unitOfWork.Vocabularies.AddPropertyAync(
            request.VocabularyId, 
            request.LocalName, 
            request.Label, 
            request.TermUri
        );

        await _unitOfWork.CommitAsync();

        return property;
    }
}