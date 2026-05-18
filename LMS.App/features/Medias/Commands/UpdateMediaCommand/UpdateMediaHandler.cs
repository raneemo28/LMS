using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.App.DTOs.Media;

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
        var media = await _unitOfWork.Media.GetMediaWithMetadataAsync(request.Dto.Id);

        if (media == null)
        {
            return false;
        }

        media.FileName = request.Dto.FileName;
        media.AltText = request.Dto.AltText;
        media.ItemId = request.Dto.ItemId;
        media.ModifiedBy = request.Dto.CurrentUserId;
        media.ModifiedAt = DateTime.UtcNow;

        if (request.Dto.Values != null)
        {
            if (media.Values != null)
            {
                media.Values.Clear();
            }
            else
            {
                media.Values = new List<Value>();
            }

            foreach (var v in request.Dto.Values)
            {
                media.Values.Add(new Value
                {
                    PropertyId = v.PropertyId,
                    ValueText = v.ValueText,
                    ValueUri = v.ValueUri,
                    ValueResourceId = request.Dto.Id,
                    Type = v.Type,
                    Language = v.Language
                });
            }
        }

        await _unitOfWork.CommitAsync();

        return true;
    }
}