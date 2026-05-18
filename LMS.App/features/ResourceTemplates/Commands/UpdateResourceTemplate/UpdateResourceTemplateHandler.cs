using MediatR;
using LMS.Domain.Interfaces;

namespace LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;

public class UpdateResourceTemplateCommandHandler : IRequestHandler<UpdateResourceTemplateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateResourceTemplateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateResourceTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _unitOfWork.ResourceTemplates.GetByIdAsync(request.Id);

        if (template == null) return false;
        if(request.Label != template.Label && !await _unitOfWork.ResourceTemplates.IsLabelUniqueAsync(request.Label)){
            throw new InvalidOperationException("Label already exists.");
        }
        template.Label = request.Label;
        template.Description = request.Description;

        _unitOfWork.ResourceTemplates.Update(template);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}
