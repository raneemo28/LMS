using MediatR;

namespace LMS.App.Features.ResourceTemplates.Commands.CreateResourseTemplate;

public record CreateResourceTemplateCommand(
    string Label,
    string? Description
) : IRequest<int>;