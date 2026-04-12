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
            CreatedBy = request.CurrentUserId,
            OwnerId = request.CurrentUserId
        };

        await _unitOfWork.Items.AddAsync(item);
        
        await _unitOfWork.CommitAsync();

        return item.Id;
    }
}