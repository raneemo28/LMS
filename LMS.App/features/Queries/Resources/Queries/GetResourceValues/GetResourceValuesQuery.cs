using MediatR;

namespace LMS.Application.Features.Resources.Queries.GetResourceValues;

public record GetResourceValuesQuery(int ResourceId) : IRequest<object>;