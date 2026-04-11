using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.ItemSets.Commands.CreateItemSets;

public class CreateItemSetHandler : IRequestHandler<CreateItemSetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateItemSetHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CreateItemSetCommand request, CancellationToken cancellationToken)
    {

        var newItemSet = new ItemSet
        {
            Title = request.Title,
            Description = request.Description,
            IsPublic = request.IsPublic
        };

        await _unitOfWork.ItemSets.AddAsync(newItemSet);

        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}