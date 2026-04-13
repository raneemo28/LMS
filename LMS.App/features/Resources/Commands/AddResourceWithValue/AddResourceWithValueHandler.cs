using MediatR;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;

namespace LMS.Application.Features.Resources.Commands.AddResourceWithValue;

public class AddResourceWithValueHandler : IRequestHandler<AddResourceWithValueCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddResourceWithValueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddResourceWithValueCommand request, CancellationToken cancellationToken)
    {
        Resource resource = request.ResourceType.ToLower() switch
        {
            "media" => new Domain.Entities.Media(),
            "item" => new Domain.Entities.Item(),
            "itemset" => new ItemSet(),
            _ => throw new ArgumentException($"النوع {request.ResourceType} غير مدعوم في النظام.")
        };

        resource.Type = request.ResourceType;
        resource.OwnerId = request.OwnerId;
        resource.CreatedAt = DateTime.Now;
        resource.CreatedBy = request.OwnerId;

        var value = new Value
        {
            PropertyId = request.PropertyId,
            ValueText = request.ValueText,
            ValueUri = request.ValueUri,
            ValueResourceId = request.ValueResourceId,
            Type = request.ValueType,
            Language = request.Language
        };


        var success = await _unitOfWork.Resources.AddResourceWithValue(resource, value);

        if (!success) return 0;

        await _unitOfWork.CommitAsync();

        return resource.Id;
    }
}