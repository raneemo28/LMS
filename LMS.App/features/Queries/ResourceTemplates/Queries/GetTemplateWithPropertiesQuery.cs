using MediatR;

namespace LMS.Application.Features.ResourceTemplates.Queries.GetTemplateWithProperties;

public record GetTemplateWithPropertiesQuery(int Id) : IRequest<object>;