using MediatR;

namespace LMS.App.features.Queries.Resources.Queries.GetResourceType;

public record GetResourceTypeQuery(int ResourceId) : IRequest<object>;