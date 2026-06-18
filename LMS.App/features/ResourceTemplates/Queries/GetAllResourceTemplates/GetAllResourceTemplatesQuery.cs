using MediatR;
using LMS.App.DTOs.ResourceTemplate;

namespace LMS.App.Features.ResourceTemplates.Queries.GetAllResourceTemplates;

public record GetAllResourceTemplatesQuery : IRequest<IEnumerable<ResourceTemplateDto>>;