using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMS.App.Features.Resources.Commands.AddValues;
using LMS.App.Features.Resources.Commands.UpdateValue;
using LMS.App.Features.Resources.Commands.RemoveValue;
using LMS.App.Features.Resources.Queries.GetResourceValues;
using LMS.App.DTOs.Value;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ResourceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    private readonly IStringLocalizer<SharedResource> _sharedLocalizer; // ADDED

    public ResourceController(
        IMediator mediator, 
        IStringLocalizer<ErrorMessages> localizer,
        IStringLocalizer<SharedResource> sharedLocalizer) // ADDED
    {
        _mediator = mediator;
        _localizer = localizer;
        _sharedLocalizer = sharedLocalizer; // ADDED
    }

    [HttpPost("{resourceId}/values")]
    public async Task<IActionResult> AddValues([FromRoute] int resourceId, [FromBody] List<CreateResourceValueDto> values)
    {
        var result = await _mediator.Send(new AddValuesCommand(resourceId, values));
        return result ? Ok(new { Success = true, Message = _sharedLocalizer["Success"] }) : BadRequest(new { Success = false, Message = _sharedLocalizer["Error"] });
    }

    [HttpPut("{resourceId}/values/{valueId}")]
    public async Task<IActionResult> UpdateValue([FromRoute] int resourceId, [FromRoute] int valueId, [FromBody] UpdateResourceValueDto dto)
    {
        var result = await _mediator.Send(new UpdateValueCommand(resourceId, valueId, dto));
        return result ? Ok(new { Success = true, Message = _sharedLocalizer["Success"] }) : BadRequest(new { Success = false, Message = _sharedLocalizer["Error"] });
    }

    [HttpDelete("{resourceId}/values/{valueId}")]
    public async Task<IActionResult> RemoveValue([FromRoute] int resourceId, [FromRoute] int valueId)
    {
        var result = await _mediator.Send(new RemoveValueCommand(resourceId, valueId));
        return result ? Ok(new { Success = true, Message = _sharedLocalizer["Success"] }) : BadRequest(new { Success = false, Message = _sharedLocalizer["Error"] });
    }

    [HttpGet("{resourceId}/values")]
    public async Task<IActionResult> GetResourceValues([FromRoute] int resourceId)
    {
        var result = await _mediator.Send(new GetResourceValuesQuery(resourceId));
        return Ok(result);
    }
}