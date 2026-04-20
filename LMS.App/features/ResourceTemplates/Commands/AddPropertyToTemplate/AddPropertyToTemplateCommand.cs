using MediatR;
namespace LMS.App.Features.ResourceTemplates.Commands.AddPropertyToTemplate;
public record AddPropertyToTemplateCommand(
    int TemplateId, 
    bool IsRequired,
    int DisplayOrder,,
    string AlternateLabel,
) : IRequest<bool>;