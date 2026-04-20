using MediatR;
using LMS.Domain.Interfaces;
namespace LMS.App.Features.ResourceTemplates.Commands.AddPropertyToTemplate;
public class AddPropertyToTemplateHandler : IRequestHandler<AddPropertyToTemplateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public AddPropertyToTemplateHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(AddPropertyToTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ResourceTemplates.AddPropertyToTemplateAsync(
            request.TemplateId, request.PropertyId, request.IsRequired, request.DisplayOrder, request.AlternateLabel);

        if (result == null) return false;

        return await _unitOfWork.CommitAsync() > 0;
    }
}