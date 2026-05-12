using MediatR;
using LMS.Domain.Interfaces;
namespace LMS.App.Features.ResourceTemplates.Commands.RemovePropertyFromTemplate;
public class RemovePropertyFromTemplateHandler : IRequestHandler<RemovePropertyFromTemplateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public RemovePropertyFromTemplateHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(RemovePropertyFromTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ResourceTemplates.RemovePropertyFromTemplateAsync(
            request.TemplateId, request.PropertyId);

        if (result == null) return false;

        return await _unitOfWork.CommitAsync() > 0;
    }
}
