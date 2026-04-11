using MediatR;
using LMS.Domain.Entities;

namespace LMS.Application.Features.Resources.Queries.GetResourcesByType;

public record GetResourcesByTypeQuery<T>() : IRequest<IEnumerable<T>> where T : Resource;