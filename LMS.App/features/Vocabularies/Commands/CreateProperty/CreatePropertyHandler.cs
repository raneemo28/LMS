using AutoMapper;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Vocabularies.Commands.CreateProperty;

public class CreatePropertyHandler : IRequestHandler<CreatePropertyCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePropertyHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreatePropertyCommand request, CancellationToken ct)
    {
        var vocab = await _unitOfWork.Vocabularies.GetByIdAsync(request.VocabularyId);
        if (vocab == null)
            throw new KeyNotFoundException($"Vocabulary ID {request.VocabularyId} not found.");

        var property = _mapper.Map<Property>(request.Dto);
        property.VocabularyId = request.VocabularyId;

        await _unitOfWork.Vocabularies.AddPropertyAsync(property);
        await _unitOfWork.CommitAsync();

        return property.Id;
    }
}