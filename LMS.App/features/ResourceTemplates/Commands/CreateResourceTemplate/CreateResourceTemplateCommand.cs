using MediatR;

namespace LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;

public record CreateResourceTemplateCommand(
    string Label,
    string? Description
) : IRequest<int>;
