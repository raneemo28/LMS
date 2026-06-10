using MediatR;
using LMS.App.DTOs.ResourceTemplate;

namespace LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;

public record CreateResourceTemplateCommand(
    CreateResourceTemplateDto Dto
) : IRequest<int>;

