using LMS.App.DTOs.Value;
using MediatR;

namespace LMS.Application.Features.Item.Commands.CreateItem;

public record CreateItemCommand(
    int TemplateId, 
    string OwnerId,
    List<CreateResourceValueDto> Values
) : IRequest<int>;