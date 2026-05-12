using MediatR;
using LMS.App.DTOs.ResourceTemplate;

namespace LMS.App.Features.ResourceTemplates.Queries.GetTemplateWithProperties;

public record GetTemplateWithPropertiesQuery(int Id) : IRequest<ResourceTemplateDto?>;
