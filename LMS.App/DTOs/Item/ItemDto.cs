using System;
using System.Collections.Generic;
using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Item;

public record ItemDto(
    int Id,
    int? TemplateId,
    List<ResourceValueDto> Values
);
/*
someHow the dto is identical with the entity

//this is how it should be handled in the controller
[HttpGet("{id}")]
public async Task<IActionResult> Get(int id)
{
    // You "give" the ID to the query here:
    var query = new GetItemWithFullDataAsyncQuery(id);
    
    var result = await _mediator.Send(query);
    return result != null ? Ok(result) : NotFound();
}

*/
