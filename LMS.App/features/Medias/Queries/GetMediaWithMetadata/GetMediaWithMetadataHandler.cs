using AutoMapper;
using LMS.App.DTOs.Media;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Medias.Queries.GetMediaWithMetadata;

public class GetMediaWithMetadataHandler : IRequestHandler<GetMediaWithMetadataQuery, MediaWithMetadataDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMediaWithMetadataHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MediaWithMetadataDto?> Handle(GetMediaWithMetadataQuery request, CancellationToken cancellationToken)
    {
        var media = await _unitOfWork.Media.GetMediaWithMetadataAsync(request.MediaId);
        if (media == null)
            throw new InvalidOperationException("Media not found.");

        if (media.ItemId == null || media.Item == null || media.Item.Id == 0)
            throw new InvalidOperationException(
                "The requested Media exists, but its associated Item does not exist in the system.");

        return _mapper.Map<MediaWithMetadataDto>(media);
    }
}