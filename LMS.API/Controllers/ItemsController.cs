using System.Linq.Expressions;
using System.Security.Claims;
using LMS.App.DTOs.Item;
using LMS.App.Features.Items.Commands.CreateItem;
using LMS.App.Features.Items.Commands.DeleteItem;
using LMS.App.Features.Items.Commands.UpdateItem;
using LMS.App.Features.Items.Queries.GetItemsWithFullDataWithConditionAsync;
using LMS.App.Features.Items.Queries.GetItemWithFullDataAsync;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;


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
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
            return Unauthorized("User ID not found in token.");

        var command = new CreateItemCommand(dto.TemplateId, ownerId, dto.Values);
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItemDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch.");

        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
            return Unauthorized("User ID not found in token.");

        var command = new UpdateItemCommand(dto.Id, dto.TemplateId, ownerId, dto.Values);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
            return Unauthorized("User ID not found in token.");

        var command = new DeleteItemCommand(id, ownerId);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetItemWithFullDataAsyncQuery(id);
        var result = await _mediator.Send(query);
        return result != null ? Ok(result) : NotFound();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? templateId = null)
    {
        Expression<Func<Item, bool>> filter = x => true;
        
        if (templateId.HasValue)
        {
            filter = x => x.TemplateId == templateId.Value;
        }

        var query = new GetItemsWithFullDataWithConditionQuery(filter);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
