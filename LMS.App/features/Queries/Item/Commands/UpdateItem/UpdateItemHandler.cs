using LMS.Domain.Interfaces;
using MediatR;

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
        var item = await _unitOfWork.Items.GetByIdAsync(request.Id) as LMS.Domain.Entities.Item;

        if (item == null) return false;

        bool isOwner = await _unitOfWork.Items.IsOwnerAsync(request.Id, request.UserId);
        bool isAdmin = request.UserRoles?.Contains("Admin") ?? false;

        if (!isOwner && !isAdmin)
        {
            throw new UnauthorizedAccessException("Unauthorized access.");
        }

        item.TemplateId = request.TemplateId;


        if (item.Template != null)
        {
            item.Template.Label = request.Title; 
            item.Template.Description = request.Description;
        }

        _unitOfWork.Items.Update(item);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}