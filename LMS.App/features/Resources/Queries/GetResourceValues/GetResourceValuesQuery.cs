using MediatR;
using LMS.App.DTOs.Value;

namespace LMS.Application.Features.Resources.Queries.GetResourceValues;

public record GetResourceValuesQuery(int ResourceId) : IRequest<List<ResourceValueDto>>;