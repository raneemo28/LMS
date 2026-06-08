using AutoMapper;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;

public class CreateResourceTemplateCommandHandler : IRequestHandler<CreateResourceTemplateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateResourceTemplateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateResourceTemplateCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.ResourceTemplates.IsLabelUniqueAsync(request.Dto.Label))
            throw new InvalidOperationException("Label already exists.");

        var template = _mapper.Map<ResourceTemplate>(request.Dto);

        await _unitOfWork.ResourceTemplates.AddAsync(template);
        await _unitOfWork.CommitAsync();

        return template.Id;
    }
}