using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LMS.App.Features.Vocabularies.Commands.CreateVocabulary;
using LMS.App.Features.Vocabularies.Commands.UpdateVocabulary;
using LMS.App.Features.Vocabularies.Commands.DeleteVocabulary;
using LMS.App.Features.Vocabularies.Commands.CreateProperty;
using LMS.App.Features.Vocabularies.Commands.UpdateProperty;
using LMS.App.Features.Vocabularies.Commands.DeleteProperty;
using LMS.App.Features.Vocabularies.Queries.GetAllVocabularies;
using LMS.App.Features.Vocabularies.Queries.GetVocabularyById;
using LMS.App.Features.Vocabularies.Queries.GetVocabularyByPrefix;
using LMS.App.DTOs.Vocabulary;
using LMS.App.DTOs.Property;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VocabulariesController : ControllerBase
{
    private readonly IMediator _mediator;
    public VocabulariesController(IMediator mediator) => _mediator = mediator;

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllVocabulariesQuery()));

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetVocabularyByIdQuery(id));
        return result is null ? NotFound(new { Message = $"Vocabulary {id} not found." }) : Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("by-prefix")]
    public async Task<IActionResult> GetByPrefix([FromQuery] string prefix)
        => Ok(await _mediator.Send(new GetVocabularyByPrefixQuery(prefix)));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVocabularyDto dto)
    {
        var id = await _mediator.Send(new CreateVocabularyCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id }, new { Id = id, Success = true });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVocabularyDto dto)
    {
        var result = await _mediator.Send(new UpdateVocabularyCommand(id, dto));
        return result
            ? Ok(new { Success = true, Message = "Vocabulary updated successfully." })
            : NotFound(new { Success = false, Message = $"Vocabulary {id} not found." });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteVocabularyCommand(id));
        return result
            ? Ok(new { Success = true, Message = "Vocabulary deleted successfully." })
            : NotFound(new { Success = false, Message = $"Vocabulary {id} not found." });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{vocabularyId:int}/properties")]
    public async Task<IActionResult> CreateProperty(int vocabularyId, [FromBody] CreatePropertyDto dto)
    {
        var id = await _mediator.Send(new CreatePropertyCommand(vocabularyId, dto));
        return CreatedAtAction(nameof(GetById), new { id = vocabularyId },
            new { PropertyId = id, Success = true, Message = "Property created successfully." });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("properties/{propertyId:int}")]
    public async Task<IActionResult> UpdateProperty(int propertyId, [FromBody] UpdatePropertyDto dto)
    {
        var result = await _mediator.Send(new UpdatePropertyCommand(propertyId, dto));
        return result
            ? Ok(new { Success = true, Message = "Property updated successfully." })
            : NotFound(new { Success = false, Message = $"Property {propertyId} not found." });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("properties/{propertyId:int}")]
    public async Task<IActionResult> DeleteProperty(int propertyId)
    {
        var result = await _mediator.Send(new DeletePropertyCommand(propertyId));
        return result
            ? Ok(new { Success = true, Message = "Property deleted successfully." })
            : NotFound(new { Success = false, Message = $"Property {propertyId} not found or has linked values." });
    }
}