using LMS.App.DTOs.ResourceTemplate;
using MediatR;

namespace LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;

public record UpdateResourceTemplateCommand(
    int Id,
    UpdateResourceTemplateDto Dto
) : IRequest<bool>;