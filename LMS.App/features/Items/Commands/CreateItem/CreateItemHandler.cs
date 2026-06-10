using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;
using AutoMapper;
namespace LMS.App.Features.Items.Commands.CreateItem;

public class CreateItemHandler : IRequestHandler<CreateItemCommand, int> 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateItemHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Item>(request.Dto);
        item.Type = "Item";
        item.CreatedAt = DateTime.UtcNow;
        item.CreatedBy = request.OwnerId;
        item.OwnerId = request.OwnerId;

        await _unitOfWork.Items.AddAsync(item);
        // This single CommitAsync saves both the Item and all its associated Values atomically.
        await _unitOfWork.CommitAsync();
         

        return item.Id;
    }
}
