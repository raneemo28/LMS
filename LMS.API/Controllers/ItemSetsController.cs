using System.Security.Claims;
using LMS.App.DTOs.ItemSet;
using LMS.App.Features.ItemSets.Commands.AddItemToSet;
using LMS.App.Features.ItemSets.Commands.CreateItemSets;
using LMS.App.Features.ItemSets.Commands.DeleteItemSets;
using LMS.App.Features.ItemSets.Commands.RemoveItemFromSet;
using LMS.App.Features.ItemSets.Commands.UpdateItemSets;
using LMS.App.Features.ItemSets.Queries.GetAllItemSets;
using LMS.App.Features.ItemSets.Queries.GetItemSetWithMembers;
using LMS.App.Features.ItemSets.Queries.GetPublicSetsAsync;
using LMS.App.Features.Queries.ItemSets.CheckItemSetOwnership;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ItemSetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemSetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemSetDto dto)
    {
        var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
            return Unauthorized("User ID not found in token.");

        var result = await _mediator.Send(new CreateItemSetCommand(dto, ownerId));
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItemSetDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        var result = await _mediator.Send(new UpdateItemSetCommand(dto, userId, userRoles));
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        var result = await _mediator.Send(new DeleteItemSetCommand(id, userId, userRoles));
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{setId}/items/{itemId}")]
    public async Task<IActionResult> AddItemToSet(int setId, int itemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var result = await _mediator.Send(new AddItemToSetCommand(setId, itemId, userId));
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{setId}/items/{itemId}")]
    public async Task<IActionResult> RemoveItemFromSet(int setId, int itemId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var result = await _mediator.Send(new RemoveItemFromSetCommand(setId, itemId, userId));
        return result ? NoContent() : NotFound();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllItemSetsQuery());
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("public")]
    public async Task<IActionResult> GetPublicSets()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
        var result = await _mediator.Send(new GetPublicSetsAsyncQuery(userId));
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetItemSetWithMembersQuery(id));
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("{id}/ownership")]
    public async Task<IActionResult> CheckOwnership(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var result = await _mediator.Send(new CheckItemSetOwnershipQuery(id, userId));
        return Ok(result);
    }
}