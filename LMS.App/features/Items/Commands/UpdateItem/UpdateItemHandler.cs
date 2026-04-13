using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;
using System.Linq; // Added for Select/ToList

namespace LMS.Application.Features.Item.Commands.UpdateItem;

public class UpdateItemHandler : IRequestHandler<UpdateItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateItemHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        // 1. IMPORTANT: Use the "FullData" method to ensure existing Values are loaded into memory.
        // This allows EF Core to track the collection and delete the old values when you replace them.
        var item = await _unitOfWork.Items.GetItemWithFullDataAsync(request.Id);

        if (item == null) return false;

        // 2. Authorization Check
        if (item.OwnerId != request.OwnerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this item.");
        }

        // 3. Update Scalar Properties
        item.TemplateId = request.TemplateId;
        item.ModifiedAt = DateTime.UtcNow;
        item.ModifiedBy = request.OwnerId;

        // 4. Update Collection (Replace All strategy)
        // Fixed: Added 'item.' prefix
        item.Values = request.Values.Select(v => new Value
        {
            PropertyId = v.PropertyId,
            ValueText = v.ValueText,
            ValueUri = v.ValueUri,
            ValueResourceId = v.ValueResourceId,
            Type = v.Type,
            Language = v.Language
        }).ToList();

        // 5. Persist
        _unitOfWork.Items.Update(item);
        var result = await _unitOfWork.CommitAsync();

        // Fixed: Convert int to bool
        return result > 0;
    }
}
