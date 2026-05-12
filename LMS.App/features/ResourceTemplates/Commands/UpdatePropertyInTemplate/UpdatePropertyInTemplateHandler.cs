using MediatR;
using LMS.Domain.Interfaces;
namespace LMS.App.Features.ResourceTemplates.Commands.UpdatePropertyInTemplate;
public class UpdatePropertyInTemplateHandler : IRequestHandler<UpdatePropertyInTemplateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdatePropertyInTemplateHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(UpdatePropertyInTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ResourceTemplates.UpdatePropertyInTemplateAsync(
            request.TemplateId, request.PropertyId, request.IsRequired, request.DisplayOrder, request.AlternateLabel);

        if (result == null) return false;

        return await _unitOfWork.CommitAsync() > 0;
    }
}
