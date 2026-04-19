using MediatR;
using LMS.App.DTOs.Media;

namespace LMS.App.Features.Media.Queries.GetMediaWithMetadata;
public record GetMediaWithMetadataQuery(int MediaId) : IRequest<MediaWithMetadataDto?>;