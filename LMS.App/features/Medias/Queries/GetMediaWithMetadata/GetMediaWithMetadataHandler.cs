using MediatR;
using LMS.App.DTOs.Media;
using LMS.Domain.Interfaces;
using AutoMapper;
namespace LMS.App.Features.Media.Queries.GetMediaWithMetadata;

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
        // استخدام الميثود المخصصة التي أضفناها لـ IMediaRepository
        var media = await _unitOfWork.Media.GetMediaWithMetadataAsync(request.MediaId);

        if (media == null) return null;

        return _mapper.Map<MediaWithMetadataDto>(media);
    }
}