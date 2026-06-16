using AutoMapper;
using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;

public class UpdateResourceTemplateCommandHandler : IRequestHandler<UpdateResourceTemplateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public UpdateResourceTemplateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<bool> Handle(UpdateResourceTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _unitOfWork.ResourceTemplates.GetByIdAsync(request.Id);
        if (template == null) return false;

        if (request.Dto.Label != template.Label && !await _unitOfWork.ResourceTemplates.IsLabelUniqueAsync(request.Dto.Label))
            throw new InvalidOperationException(_localizer["LabelAlreadyExists"]);

        _mapper.Map(request.Dto, template);
        _unitOfWork.ResourceTemplates.Update(template);
        return await _unitOfWork.CommitAsync() > 0;
    }
}