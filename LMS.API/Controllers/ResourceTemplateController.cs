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
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;
using LMS.App.Features.ResourceTemplates.Queries.GetAllResourceTemplates;

namespace LMS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ResourceTemplateController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    private readonly IStringLocalizer<SharedResource> _sharedLocalizer; 

    public ResourceTemplateController(
        IMediator mediator, 
        IStringLocalizer<ErrorMessages> localizer,
        IStringLocalizer<SharedResource> sharedLocalizer) 
    {
        _mediator = mediator;
        _localizer = localizer;
        _sharedLocalizer = sharedLocalizer; 
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllResourceTemplatesQuery());
        return Ok(result);
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
        return result == null ? NotFound(new { Success = false, Message = _localizer["ResourceNotFound"] }) : Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTemplate([FromRoute] int id, [FromBody] UpdateResourceTemplateDto dto)
    {
        var result = await _mediator.Send(new UpdateResourceTemplateCommand(id, dto));
        return result ? Ok(new { Success = true, Message = _sharedLocalizer["Success"] }) : BadRequest(new { Success = false, Message = _sharedLocalizer["Error"] }); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate([FromRoute] int id)
    {
        var result = await _mediator.Send(new DeleteResourceTemplateCommand(id));
        return result ? Ok(new { Success = true, Message = _sharedLocalizer["Success"] }) : BadRequest(new { Success = false, Message = _sharedLocalizer["Error"] }); 
    }

    [HttpPost("{templateId}/properties")]
    public async Task<IActionResult> AddPropertiesToTemplate([FromRoute] int templateId, [FromBody] List<PropertyToTemplateInput> properties)
    {
        var result = await _mediator.Send(new AddPropertiesToTemplateCommand(templateId, properties));
        return result ? Ok(new { Success = true, Message = _sharedLocalizer["Success"] }) : BadRequest(new { Success = false, Message = _sharedLocalizer["Error"] }); 
    }

    [HttpPut("{templateId}/properties/{propertyId}")]
    public async Task<IActionResult> UpdatePropertyInTemplate([FromRoute] int templateId, [FromRoute] int propertyId, [FromBody] UpdatePropertyInTemplateDto dto)
    {
        var result = await _mediator.Send(new UpdatePropertyInTemplateCommand(templateId, propertyId, dto.IsRequired, dto.DisplayOrder, dto.AlternateLabel));
        return result ? Ok(new { Success = true, Message = _sharedLocalizer["Success"] }) : BadRequest(new { Success = false, Message = _sharedLocalizer["Error"] }); 
    }

    [HttpDelete("{templateId}/properties/{propertyId}")]
    public async Task<IActionResult> RemovePropertyFromTemplate([FromRoute] int templateId, [FromRoute] int propertyId)
    {
        var result = await _mediator.Send(new RemovePropertyFromTemplateCommand(templateId, propertyId));
        return result ? Ok(new { Success = true, Message = _sharedLocalizer["Success"] }) : BadRequest(new { Success = false, Message = _sharedLocalizer["Error"] }); 
    }
}