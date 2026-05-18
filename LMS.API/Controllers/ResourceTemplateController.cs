using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;
using LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;
using LMS.App.Features.ResourceTemplates.Commands.DeleteResourceTemplete;
using LMS.App.Features.ResourceTemplates.Commands.AddPropertiesToTemplate;
using System.Collections.Generic;
using System.Linq;
using LMS.App.Features.ResourceTemplates.Commands.UpdatePropertyInTemplate;
using LMS.App.Features.ResourceTemplates.Commands.RemovePropertyFromTemplate;
using LMS.App.Features.ResourceTemplates.Queries.GetTemplateWithProperties;
using LMS.App.DTOs.ResourceTemplate;
using Microsoft.AspNetCore.Authorization;

namespace LMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ResourceTemplateController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResourceTemplateController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateResourceTemplateRequest req)
    {
        var command = new CreateResourceTemplateCommand(req.Label, req.Description);
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTemplate), new { id }, new { Id = id, Success = true });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplate([FromRoute] int id)
    {
        var query = new GetTemplateWithPropertiesQuery(id);
        var result = await _mediator.Send(query);
        if (result == null) return NotFound(new { Success = false, Message = "Resource template not found." });
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTemplate([FromRoute] int id, [FromBody] UpdateResourceTemplateRequest req)
    {
        var command = new UpdateResourceTemplateCommand(id, req.Label, req.Description);
        var result = await _mediator.Send(command);
        return result ? Ok(new { Success = true, Message = "Template updated successfully." }) : BadRequest(new { Success = false, Message = "Failed to update template." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate([FromRoute] int id)
    {
        var command = new DeleteResourceTemplateCommand(id);
        var result = await _mediator.Send(command);
        return result ? Ok(new { Success = true, Message = "Template deleted successfully." }) : BadRequest(new { Success = false, Message = "Failed to delete template." });
    }

    [HttpPost("{templateId}/properties")]
    public async Task<IActionResult> AddPropertiesToTemplate([FromRoute] int templateId, [FromBody] AddPropertiesToTemplateRequest req)
    {
        var command = new AddPropertiesToTemplateCommand(
            templateId,
            req.Properties.Select(p => new PropertyToTemplateInput(
                p.PropertyId,
                p.IsRequired,
                p.DisplayOrder,
                p.AlternateLabel
            )).ToList()
        );
        var result = await _mediator.Send(command);
        return result ? Ok(new { Success = true, Message = "Properties added to template successfully." }) : BadRequest(new { Success = false, Message = "Failed to add properties to template." });
    }

    [HttpPut("{templateId}/properties/{propertyId}")]
    public async Task<IActionResult> UpdatePropertyInTemplate(
        [FromRoute] int templateId,
        [FromRoute] int propertyId,
        [FromBody] UpdatePropertyInTemplateRequest req)
    {
        var command = new UpdatePropertyInTemplateCommand(
            templateId,
            propertyId,
            req.IsRequired,
            req.DisplayOrder,
            req.AlternateLabel
        );
        var result = await _mediator.Send(command);
        return result ? Ok(new { Success = true, Message = "Template property updated successfully." }) : BadRequest(new { Success = false, Message = "Failed to update template property." });
    }

    [HttpDelete("{templateId}/properties/{propertyId}")]
    public async Task<IActionResult> RemovePropertyFromTemplate([FromRoute] int templateId, [FromRoute] int propertyId)
    {
        var command = new RemovePropertyFromTemplateCommand(templateId, propertyId);
        var result = await _mediator.Send(command);
        return result ? Ok(new { Success = true, Message = "Property removed from template successfully." }) : BadRequest(new { Success = false, Message = "Failed to remove property from template." });
    }
}

public record CreateResourceTemplateRequest(string Label, string? Description);
public record UpdateResourceTemplateRequest(string Label, string? Description);
public record PropertyToTemplateRequest(int PropertyId, bool IsRequired, int DisplayOrder, string? AlternateLabel);
public record AddPropertiesToTemplateRequest(List<PropertyToTemplateRequest> Properties);
public record UpdatePropertyInTemplateRequest(bool IsRequired, int DisplayOrder, string? AlternateLabel);
