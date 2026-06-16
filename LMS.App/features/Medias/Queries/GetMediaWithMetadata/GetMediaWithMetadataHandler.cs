using AutoMapper;
using LMS.App.DTOs.Media;
using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Medias.Queries.GetMediaWithMetadata;

public class GetMediaWithMetadataHandler : IRequestHandler<GetMediaWithMetadataQuery, MediaWithMetadataDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public GetMediaWithMetadataHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<MediaWithMetadataDto?> Handle(GetMediaWithMetadataQuery request, CancellationToken cancellationToken)
    {
        var media = await _unitOfWork.Media.GetMediaWithMetadataAsync(request.MediaId);
        if (media == null)
            throw new InvalidOperationException(_localizer["MediaNotFound"]);

        if (media.ItemId == null || media.Item == null || media.Item.Id == 0)
            throw new InvalidOperationException(_localizer["ResourceNotFound"]);

        return _mapper.Map<MediaWithMetadataDto>(media);
    }
}