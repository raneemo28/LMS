using MediatR;

namespace LMS.App.Features.ResourceTemplates.Commands.DeleteResourceTemplete;

public record DeleteResourceTemplateCommand(int Id) : IRequest<bool>;
