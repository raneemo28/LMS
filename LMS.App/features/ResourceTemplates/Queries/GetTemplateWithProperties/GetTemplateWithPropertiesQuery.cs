using MediatR;
using LMS.App.DTOs.ResourceTemplate;

namespace LMS.Application.Features.ResourceTemplates.Queries.GetTemplateWithProperties;

public record GetTemplateWithPropertiesQuery(int Id) : IRequest<ResourceTemplateDto>;