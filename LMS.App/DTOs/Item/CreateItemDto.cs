using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Item;


public record CreateItemDto(
    int TemplateId,
    List<CreateResourceValueDto> Values
);
/*
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;
using LMS.App.DTOs.Item;
using LMS.Application.Features.Item.Commands.CreateItem;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemDto dto)
    {
        // 1. Extract OwnerId from the token claims
        // ClaimTypes.NameIdentifier corresponds to the 'sub' claim in JWT
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized("User ID not found in token.");
        }

        // 2. Map DTO and OwnerId to the Command
        var command = new CreateItemCommand(
            dto.TemplateId, // Pivot value or validation check
            ownerId,
            dto.Values
        );

        // 3. Send through Mediator
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}

*/