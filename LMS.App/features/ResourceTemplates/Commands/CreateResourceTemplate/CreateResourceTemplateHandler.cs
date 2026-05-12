using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;

namespace LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;

public class CreateResourceTemplateCommandHandler : IRequestHandler<CreateResourceTemplateCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateResourceTemplateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateResourceTemplateCommand request, CancellationToken cancellationToken)
    {
         if(!await _unitOfWork.ResourceTemplates.IsLabelUniqueAsync(request.Label)){
            throw new Exception("Label already exists");
        }
        var template = new ResourceTemplate
        {
            Label = request.Label,
            Description = request.Description
        };

        await _unitOfWork.ResourceTemplates.AddAsync(template);
        await _unitOfWork.CommitAsync();

        return template.Id;
    }
}
