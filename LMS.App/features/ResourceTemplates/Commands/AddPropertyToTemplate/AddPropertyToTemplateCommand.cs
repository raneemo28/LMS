using MediatR;
namespace LMS.App.Features.ResourceTemplates.Commands.AddPropertyToTemplate;
public record AddPropertyToTemplateCommand(
    int TemplateId, 
    string LocalName, 
    string Label, 
    string TermUri
) : IRequest<bool>;