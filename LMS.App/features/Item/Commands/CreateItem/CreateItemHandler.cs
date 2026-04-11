using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Item.Commands.CreateItem;

public class CreateItemHandler : IRequestHandler<CreateItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateItemHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var newItem = new LMS.Domain.Entities.Item
        {
            TemplateId = request.TemplateId
        };

        await _unitOfWork.Items.AddAsync(newItem);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}