using MediatR;
using LMS.App.DTOs.Value;

namespace LMS.App.Features.Resources.Queries.GetResourceValues;

public record GetResourceValuesQuery(int ResourceId) : IRequest<List<ResourceValueDto>>;
