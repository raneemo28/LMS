using MediatR;
using LMS.App.DTOs.Value; // Ensure this matches your DTO namespace

namespace LMS.Application.Features.Item.Commands.UpdateItem;

public record UpdateItemCommand(
    int Id, 
    int TemplateId,     
    string? OwnerId,
    List<UpdateResourceValueDto> Values
) : IRequest<bool>; // Added the missing semicolon here
