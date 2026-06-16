using AutoMapper;
using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Medias.Commands.UpdateMediaCommand;

public class UpdateMediaCommandHandler : IRequestHandler<UpdateMediaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public UpdateMediaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<bool> Handle(UpdateMediaCommand request, CancellationToken cancellationToken)
    {
        var media = await _unitOfWork.Media.GetMediaWithMetadataAsync(request.Dto.Id);
        if (media == null) return false;

        if (media.OwnerId != request.CurrentUserId)
            throw new UnauthorizedAccessException(_localizer["NotAuthorizedUpdateMedia"]);

        _mapper.Map(request.Dto, media);
        media.ModifiedAt = DateTime.UtcNow;
        media.ModifiedBy = request.CurrentUserId;
        _unitOfWork.Media.Update(media);
        await _unitOfWork.CommitAsync();
        return true;
    }
}