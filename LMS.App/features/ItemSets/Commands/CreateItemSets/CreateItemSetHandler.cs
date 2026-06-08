using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;
using AutoMapper;

namespace LMS.App.Features.ItemSets.Commands.CreateItemSets;

public class CreateItemSetHandler : IRequestHandler<CreateItemSetCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateItemSetHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<int> Handle(CreateItemSetCommand request, CancellationToken cancellationToken)
    {
        var itemSet = _mapper.Map<ItemSet>(request.Dto);
        itemSet.Type = "ItemSet";
        itemSet.CreatedAt = DateTime.UtcNow;
        itemSet.CreatedBy = request.OwnerId;
        itemSet.OwnerId = request.OwnerId;

        await _unitOfWork.ItemSets.AddAsync(itemSet);

        await _unitOfWork.CommitAsync();

        return itemSet.Id;
    }
}
