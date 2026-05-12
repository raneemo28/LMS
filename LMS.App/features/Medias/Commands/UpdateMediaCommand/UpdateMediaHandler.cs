using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;

namespace LMS.App.Features.Medias.Commands.UpdateMediaCommand;

public class UpdateMediaCommandHandler : IRequestHandler<UpdateMediaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMediaCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateMediaCommand request, CancellationToken cancellationToken)
    {
        var media = await _unitOfWork.Media.GetMediaWithMetadataAsync(request.Id);
        
        if (media == null) return false;

        if (media.CreatedBy != request.CurrentUserId)
        {
            throw new UnauthorizedAccessException("Don't have permission to edit this file.");
        }

        media.FileName = request.FileName;
        media.AltText = request.AltText;
        media.ItemId = request.ItemId;
        
        media.ModifiedAt = DateTime.UtcNow;
        media.ModifiedBy = request.CurrentUserId;

        if (request.Values != null)
        {
            media.Values.Clear();

            foreach (var v in request.Values)
            {
                media.Values.Add(new Value
                {
                    ResourceId = media.Id,
                    PropertyId = v.PropertyId,
                    ValueText = v.ValueText,
                    ValueUri = v.ValueUri,
                    ValueResourceId = v.ValueResourceId,
                    Type = v.Type,
                    Language = v.Language
                });
            }
        }

        _unitOfWork.Media.Update(media);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}
