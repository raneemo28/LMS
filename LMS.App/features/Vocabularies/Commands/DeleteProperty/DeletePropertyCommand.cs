using MediatR;
namespace LMS.App.Features.Vocabularies.Commands.DeleteProperty;
public record DeletePropertyCommand(int Id) : IRequest<bool>;
