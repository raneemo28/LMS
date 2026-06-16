using AutoMapper;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;

public class CreateResourceTemplateCommandHandler : IRequestHandler<CreateResourceTemplateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public CreateResourceTemplateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<int> Handle(CreateResourceTemplateCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.ResourceTemplates.IsLabelUniqueAsync(request.Dto.Label))
            throw new InvalidOperationException(_localizer["LabelAlreadyExists"]);

        var template = _mapper.Map<ResourceTemplate>(request.Dto);
        await _unitOfWork.ResourceTemplates.AddAsync(template);
        await _unitOfWork.CommitAsync();
        return template.Id;
    }
}