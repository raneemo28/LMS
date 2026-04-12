using MediatR;
namespace LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;
public record UpdatePropertyInTemplateCommand(
    int TemplateId, 
    int PropertyId, 
    string LocalName, 
    string Label, 
    string TermUri
) : IRequest<bool>;