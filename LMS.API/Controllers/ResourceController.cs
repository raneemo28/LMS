using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.App.Features.Resources.Commands.AddValues;
using LMS.App.Features.Resources.Commands.UpdateValue;
using LMS.App.Features.Resources.Commands.RemoveValue;
using LMS.App.Features.Resources.Queries.GetResourceValues;
using LMS.App.DTOs.Value;
using Microsoft.AspNetCore.Authorization;

namespace LMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ResourceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResourceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{resourceId}/values")]
    public async Task<IActionResult> AddValues([FromRoute] int resourceId, [FromBody] List<CreateResourceValueDto> values)
    {
        var command = new AddValuesCommand(resourceId, values);
        var result = await _mediator.Send(command);
        return result ? Ok(new { Success = true, Message = "Values added successfully." }) : BadRequest(new { Success = false, Message = "Failed to add values." });
    }

    [HttpPut("{resourceId}/values/{valueId}")]
    public async Task<IActionResult> UpdateValue(
        [FromRoute] int resourceId,
        [FromRoute] int valueId,
        [FromBody] UpdateResourceValueRequest req)
    {
        var command = new UpdateValueCommand(
            resourceId,
            valueId,
            req.ValueText,
            req.ValueResourceId,
            req.Type,
            req.Language
        );
        var result = await _mediator.Send(command);
        return result ? Ok(new { Success = true, Message = "Value updated successfully." }) : BadRequest(new { Success = false, Message = "Failed to update value." });
    }

    [HttpDelete("{resourceId}/values/{valueId}")]
    public async Task<IActionResult> RemoveValue([FromRoute] int resourceId, [FromRoute] int valueId)
    {
        var command = new RemoveValueCommand(resourceId, valueId);
        var result = await _mediator.Send(command);
        return result ? Ok(new { Success = true, Message = "Value removed successfully." }) : BadRequest(new { Success = false, Message = "Failed to remove value." });
    }

    [HttpGet("{resourceId}/values")]
    public async Task<IActionResult> GetResourceValues([FromRoute] int resourceId)
    {
        var query = new GetResourceValuesQuery(resourceId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

public record UpdateResourceValueRequest(
    string? ValueText,
    int? ValueResourceId,
    string Type,
    string? Language
);
