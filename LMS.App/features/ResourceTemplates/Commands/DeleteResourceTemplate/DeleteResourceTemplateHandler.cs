using MediatR;
using LMS.Domain.Interfaces;

namespace LMS.App.Features.ResourceTemplates.Commands.DeleteResourceTemplete;

public class DeleteResourceTemplateCommandHandler : IRequestHandler<DeleteResourceTemplateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteResourceTemplateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteResourceTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _unitOfWork.ResourceTemplates.GetByIdAsync(request.Id);

        if (template == null) return false;

        _unitOfWork.ResourceTemplates.Delete(template);

        return await _unitOfWork.CommitAsync() > 0;
    }
}
