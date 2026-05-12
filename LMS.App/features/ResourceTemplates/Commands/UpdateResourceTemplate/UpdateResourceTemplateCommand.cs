using MediatR;

namespace LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;

public record UpdateResourceTemplateCommand(
    int Id,
    string Label,
    string? Description
) : IRequest<bool>;
