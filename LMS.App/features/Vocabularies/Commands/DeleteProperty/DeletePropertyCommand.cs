using MediatR;
namespace LMS.Application.Features.Vocabularies.Commands.DeleteProperty;
public record DeletePropertyCommand(int Id) : IRequest<bool>;