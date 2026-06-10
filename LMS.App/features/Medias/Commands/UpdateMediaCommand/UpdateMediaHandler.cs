using AutoMapper;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Medias.Commands.UpdateMediaCommand;

public class UpdateMediaCommandHandler : IRequestHandler<UpdateMediaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMediaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateMediaCommand request, CancellationToken cancellationToken)
    {
        var media = await _unitOfWork.Media.GetMediaWithMetadataAsync(request.Dto.Id);
        if (media == null) return false;

        if (media.OwnerId != request.CurrentUserId)
            throw new UnauthorizedAccessException("You are not authorized to update this media.");

        _mapper.Map(request.Dto, media);
        media.ModifiedAt = DateTime.UtcNow;
        media.ModifiedBy = request.CurrentUserId;

        _unitOfWork.Media.Update(media);
        await _unitOfWork.CommitAsync();
        return true;
    }
}