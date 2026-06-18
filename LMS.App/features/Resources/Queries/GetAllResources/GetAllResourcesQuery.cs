using MediatR;
using LMS.App.DTOs.Resource;

namespace LMS.App.Features.Resources.Queries.GetAllResources;

public record GetAllResourcesQuery : IRequest<IEnumerable<ResourceDto>>;