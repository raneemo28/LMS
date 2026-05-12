using MediatR;
using LMS.App.DTOs.Value; // Ensure this matches your DTO namespace

namespace LMS.App.Features.Items.Commands.UpdateItem;

public record UpdateItemCommand(
    int Id, 
    int TemplateId,     
    string OwnerId,
    List<ResourceValueDto> Values
) : IRequest<bool>; 
