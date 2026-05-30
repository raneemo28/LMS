using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;
using System.Linq; // Added for Select/ToList

namespace LMS.App.Features.Items.Commands.UpdateItem;

public class UpdateItemHandler : IRequestHandler<UpdateItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateItemHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        
        var item = await _unitOfWork.Items.GetItemWithFullDataForUpdateAsync(request.Id);

        if (item == null) return false;

        if (item.OwnerId != request.OwnerId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this item.");
        }

        item.TemplateId = request.TemplateId;
        item.ModifiedAt = DateTime.UtcNow;
        item.ModifiedBy = request.OwnerId;
        
        item.Values.Clear(); 
        
        var newValues = request.Values.Select(v => new Value
        {
            PropertyId = v.PropertyId,
            ValueText = v.ValueText,
            ValueUri = v.ValueUri,
            ValueResourceId = v.ValueResourceId,
            Type = v.Type,
            Language = v.Language
        }).ToList();

        foreach (var val in newValues)
        {
            item.Values.Add(val);
        }

        _unitOfWork.Items.Update(item);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}
