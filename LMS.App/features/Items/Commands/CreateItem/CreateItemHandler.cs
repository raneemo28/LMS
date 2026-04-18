using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Item.Commands.CreateItem;

public class CreateItemHandler : IRequestHandler<CreateItemCommand, int> 
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateItemHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = new Domain.Entities.Item
        {
            TemplateId = request.TemplateId,

            Type = "Item",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.OwnerId,
            OwnerId = request.OwnerId,

            // Mapping the list of value DTOs directly to the Item's Values collection.
            // EF Core will automatically handle assigning the new ItemId to these values
            // during the save process!
            Values = request.Values.Select(v => new Value
            {
                PropertyId = v.PropertyId,
                ValueText = v.ValueText,
                ValueUri = v.ValueUri,
                ValueResourceId = v.ValueResourceId,
                Type = v.Type,
                Language = v.Language
            }).ToList()
        };

        await _unitOfWork.Items.AddAsync(item);
        // This single CommitAsync saves both the Item and all its associated Values atomically.
        await _unitOfWork.CommitAsync();
         

        return item.Id;
    }
}