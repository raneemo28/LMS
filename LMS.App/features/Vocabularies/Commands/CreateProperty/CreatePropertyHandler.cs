using AutoMapper;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Vocabularies.Commands.CreateProperty;

public class CreatePropertyHandler : IRequestHandler<CreatePropertyCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public CreatePropertyHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreatePropertyCommand request, CancellationToken ct)
    {
        var vocab = await _unitOfWork.Vocabularies.GetByIdAsync(request.VocabularyId);
        if (vocab == null)
            throw new KeyNotFoundException(_localizer["ResourceNotFound"]);

        var property = _mapper.Map<Property>(request.Dto);
        property.VocabularyId = request.VocabularyId;
        await _unitOfWork.Vocabularies.AddPropertyAsync(property);
        await _unitOfWork.CommitAsync();
        return property.Id;
    }
}