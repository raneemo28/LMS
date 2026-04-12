using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;

namespace LMS.Application.Features.ItemSets.Commands.CreateItemSets;

public class CreateItemSetHandler : IRequestHandler<CreateItemSetCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateItemSetHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateItemSetCommand request, CancellationToken cancellationToken)
    {
        var itemSet = new ItemSet
        {
            Title = request.Title,
            Description = request.Description,
            IsPublic = request.IsPublic,

            Type = "ItemSet", 
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.CreatedBy,
            OwnerId = request.OwnerId
        };

        await _unitOfWork.ItemSets.AddAsync(itemSet);

        await _unitOfWork.CommitAsync();

        return itemSet.Id;
    }
}