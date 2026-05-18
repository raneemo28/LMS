using MediatR;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;

namespace LMS.App.Features.Media.Commands.CreateMediaCommand;

public class CreateMediaHandler : IRequestHandler<CreateMediaCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMediaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateMediaCommand request, CancellationToken cancellationToken)
    {
        var media = new Domain.Entities.Media
        {
            Type = "Media",
            CreatedBy = request.OwnerId,
            OwnerId = request.OwnerId,
            CreatedAt = DateTime.UtcNow,
            
            ItemId = request.ItemId,
            FileName = request.FileName,
            AltText = request.AltText,
            Values = request.Values.Select(v => new Value
            {
                PropertyId = v.PropertyId,
                ValueText = v.ValueText,
                ValueUri = v.ValueUri,
                ValueResourceId = v.ValueResourceId,
                Type = v.Type,
                Language = v.Language
            }).ToList()
        };

        await _unitOfWork.Media.AddAsync(media);

        await _unitOfWork.CommitAsync();

        return media.Id;
    }
}
