using MediatR;
namespace LMS.App.Features.ResourceTemplates.Commands.RemovePropertyFromTemplate;
public record RemovePropertyFromTemplateCommand(int TemplateId, int PropertyId) : IRequest<bool>;
