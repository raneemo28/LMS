using MediatR;
using LMS.Domain.Entities;

namespace LMS.Application.Features.Resources.Queries.GetResourcesByTypeName;

public record GetResourcesByTypeQuery(string TypeName) : IRequest<IEnumerable<Resource>>;