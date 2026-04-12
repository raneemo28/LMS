using MediatR;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
namespace LMS.App.Features.Media.Commands.CreateMediaCommand;
public class CreateMediaCommandHandler : IRequestHandler<CreateMediaCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMediaCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateMediaCommand request, CancellationToken cancellationToken)
{
    var media = new Domain.Entities.Media
    {
        FileName = request.FileName,
        StoragePath = request.StoragePath,
        MimeType = request.MimeType,
        FileSize = request.FileSize,
        ItemId = request.ItemId,
        AltText = request.AltText,
        OwnerId = request.OwnerId,
        CreatedBy = request.CreatedBy, 
        CreatedAt = DateTime.UtcNow,
        Type = nameof(Media)
    };

    await _unitOfWork.Media.AddAsync(media);
    await _unitOfWork.CommitAsync();

    return media.Id;
}
}