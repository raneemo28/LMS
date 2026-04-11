using LMS.Domain.Entities;
using MediatR;

namespace LMS.Application.Features.Item.Commands.CreateItem;
public record CreateItemCommand(int TemplateId) : IRequest<bool>;