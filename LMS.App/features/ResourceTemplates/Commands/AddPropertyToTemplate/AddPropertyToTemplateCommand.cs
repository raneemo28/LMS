using MediatR;
namespace LMS.App.Features.ResourceTemplates.Commands.AddPropertyToTemplate;
public record AddPropertyToTemplateCommand(
    int TemplateId,
    int PropertyId,
    bool IsRequired,
    int DisplayOrder,
    string AlternateLabel
) : IRequest<bool>;