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

[Authorize]
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

        var result = await _mediator.Send(new CreateItemCommand(dto, ownerId));
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

        var result = await _mediator.Send(new UpdateItemCommand(dto, ownerId));
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
            return Unauthorized("User ID not found in token.");

        var result = await _mediator.Send(new DeleteItemCommand(id, ownerId));
        return result ? NoContent() : NotFound();
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetItemWithFullDataAsyncQuery(id));
        return result != null ? Ok(result) : NotFound();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? templateId = null)
    {
        Expression<Func<Item, bool>> filter = x => true;
        if (templateId.HasValue)
            filter = x => x.TemplateId == templateId.Value;

        var result = await _mediator.Send(new GetItemsWithFullDataWithConditionQuery(filter));
        return Ok(result);
    }
}