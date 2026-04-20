using MediatR;
namespace LMS.App.Features.ResourceTemplates.Commands.UpdatePropertyInTemplate;
public record UpdatePropertyInTemplateCommand(
    int TemplateId, 
    int PropertyId, 
    bool IsRequired,
    int DisplayOrder,
    string? AlternateLabel
    ) : IRequest<bool>;