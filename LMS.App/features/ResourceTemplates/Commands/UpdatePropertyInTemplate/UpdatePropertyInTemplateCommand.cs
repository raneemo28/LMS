using MediatR;
namespace LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;
public record UpdatePropertyInTemplateCommand(
    int TemplateId, 
    int PropertyId, 
    bool IsRequired,
    int DisplayOrder,
    string? AlternateLabel
    ) : IRequest<bool>;