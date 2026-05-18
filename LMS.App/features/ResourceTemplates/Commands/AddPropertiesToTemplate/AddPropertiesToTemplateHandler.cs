using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LMS.Domain.Interfaces;

namespace LMS.App.Features.ResourceTemplates.Commands.AddPropertiesToTemplate;

public class AddPropertiesToTemplateHandler : IRequestHandler<AddPropertiesToTemplateCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddPropertiesToTemplateHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AddPropertiesToTemplateCommand request, CancellationToken cancellationToken)
    {
        foreach (var prop in request.Properties)
        {
            var result = await _unitOfWork.ResourceTemplates.AddPropertyToTemplateAsync(
                request.TemplateId,
                prop.PropertyId,
                prop.IsRequired,
                prop.DisplayOrder,
                prop.AlternateLabel
            );

            if (result == null)
            {
                return false;
            }
        }

        return await _unitOfWork.CommitAsync() > 0;
    }
}
