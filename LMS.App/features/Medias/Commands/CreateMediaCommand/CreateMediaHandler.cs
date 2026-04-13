using MediatR;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
namespace LMS.App.Features.Media.Commands.CreateMediaCommand;
public class CreateMediaHandler : IRequestHandler<CreateMediaCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateMediaHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<int> Handle(CreateMediaCommand request, CancellationToken cancellationToken)
    {
        var media = new Domain.Entities.Media
        {
            ItemId = request.ItemId,
            AltText = request.AltText,
            CreatedBy = request.OwnerId,
            
            FileName = "pending",
            StoragePath = "pending" 
        };

        await _unitOfWork.Media.AddAsync(media);
        await _unitOfWork.CommitAsync();

        return media.Id; 
    }
}