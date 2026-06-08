using Microsoft.AspNetCore.Mvc;
using MediatR;
using LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;
using LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;
using LMS.App.Features.ResourceTemplates.Commands.DeleteResourceTemplete;
using LMS.App.Features.ResourceTemplates.Commands.AddPropertiesToTemplate;
using LMS.App.Features.ResourceTemplates.Commands.UpdatePropertyInTemplate;
using LMS.App.Features.ResourceTemplates.Commands.RemovePropertyFromTemplate;
using LMS.App.Features.ResourceTemplates.Queries.GetTemplateWithProperties;
using LMS.App.DTOs.ResourceTemplate;
using LMS.App.DTOs.ResourceProperty;
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
    public async Task<IActionResult> CreateTemplate([FromBody] CreateResourceTemplateDto dto)
    {
        var id = await _mediator.Send(new CreateResourceTemplateCommand(dto));
        return CreatedAtAction(nameof(GetTemplate), new { id }, new { Id = id, Success = true });
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplate([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetTemplateWithPropertiesQuery(id));
        return result == null
            ? NotFound(new { Success = false, Message = "Resource template not found." })
            : Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTemplate([FromRoute] int id, [FromBody] UpdateResourceTemplateDto dto)
    {
        var result = await _mediator.Send(new UpdateResourceTemplateCommand(id, dto));
        return result
            ? Ok(new { Success = true, Message = "Template updated successfully." })
            : BadRequest(new { Success = false, Message = "Failed to update template." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate([FromRoute] int id)
    {
        var result = await _mediator.Send(new DeleteResourceTemplateCommand(id));
        return result
            ? Ok(new { Success = true, Message = "Template deleted successfully." })
            : BadRequest(new { Success = false, Message = "Failed to delete template." });
    }

    [HttpPost("{templateId}/properties")]
    public async Task<IActionResult> AddPropertiesToTemplate(
        [FromRoute] int templateId,
        [FromBody] List<PropertyToTemplateInput> properties)
    {
        var result = await _mediator.Send(new AddPropertiesToTemplateCommand(templateId, properties));
        return result
            ? Ok(new { Success = true, Message = "Properties added to template successfully." })
            : BadRequest(new { Success = false, Message = "Failed to add properties to template." });
    }

    [HttpPut("{templateId}/properties/{propertyId}")]
    public async Task<IActionResult> UpdatePropertyInTemplate(
        [FromRoute] int templateId,
        [FromRoute] int propertyId,
        [FromBody] UpdatePropertyInTemplateDto dto)
    {
        var result = await _mediator.Send(
            new UpdatePropertyInTemplateCommand(templateId, propertyId, dto.IsRequired, dto.DisplayOrder, dto.AlternateLabel));
        return result
            ? Ok(new { Success = true, Message = "Template property updated successfully." })
            : BadRequest(new { Success = false, Message = "Failed to update template property." });
    }

    [HttpDelete("{templateId}/properties/{propertyId}")]
    public async Task<IActionResult> RemovePropertyFromTemplate([FromRoute] int templateId, [FromRoute] int propertyId)
    {
        var result = await _mediator.Send(new RemovePropertyFromTemplateCommand(templateId, propertyId));
        return result
            ? Ok(new { Success = true, Message = "Property removed from template successfully." })
            : BadRequest(new { Success = false, Message = "Failed to remove property from template." });
    }
}