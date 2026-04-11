using MediatR;
using LMS.Domain.Entities;

namespace LMS.Application.Features.Resources.Queries.GetResourcesByTypeName;

public record GetResourcesByTypeNameQuery(string TypeName) : IRequest<IEnumerable<Resource>>;