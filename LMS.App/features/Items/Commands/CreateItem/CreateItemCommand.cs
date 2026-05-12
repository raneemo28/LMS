using LMS.App.DTOs.Value;
using MediatR;

namespace LMS.App.Features.Items.Commands.CreateItem;

public record CreateItemCommand(
    int TemplateId, 
    string OwnerId,
    List<CreateResourceValueDto> Values
) : IRequest<int>;
