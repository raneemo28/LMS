using MediatR;
using AutoMapper;
using LMS.App.DTOs.Media;
using LMS.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

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

        if (media == null)  throw new InvalidOperationException("media not found.");


        if (media.ItemId == null || media.Item == null || media.Item.Id == 0)
        {
            throw new InvalidOperationException("The requested Media exists, but its associated Item does not exist in the system.");
        }

        var metadataValues = new List<MetadataValueDto>();
        if (media.Values != null && media.Values.Any())
        {
            metadataValues = _mapper.Map<List<MetadataValueDto>>(media.Values);
        }

        return new MediaWithMetadataDto(
            media.Id,
            media.FileName ?? string.Empty,
            media.StoragePath ?? string.Empty,
            media.FileSize ?? 0,
            media.MimeType ?? "application/octet-stream",
            metadataValues
        );
    }
}