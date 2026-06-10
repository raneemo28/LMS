using AutoMapper;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;

public class UpdateResourceTemplateCommandHandler : IRequestHandler<UpdateResourceTemplateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateResourceTemplateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateResourceTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _unitOfWork.ResourceTemplates.GetByIdAsync(request.Id);
        if (template == null) return false;

        if (request.Dto.Label != template.Label &&
            !await _unitOfWork.ResourceTemplates.IsLabelUniqueAsync(request.Dto.Label))
            throw new InvalidOperationException("Label already exists.");

        _mapper.Map(request.Dto, template);

        _unitOfWork.ResourceTemplates.Update(template);
        return await _unitOfWork.CommitAsync() > 0;
    }
}