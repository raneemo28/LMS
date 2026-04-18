using LMS.App.DTOs.Value;

namespace LMS.App.DTOs.Item;

public record UpdateItemDto(
    int Id,
    int TemplateId,
    List<ResourceValueDto> Values
);
/*
[HttpPut("{id}")]
public async Task<IActionResult> Put(int id,UpdateItemDto dto)
{
    /var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(ownerId))
        {
            return Unauthorized("User ID not found in token.");
        }
    var command = new UpdateItemCommand(
            dto.Id,
            dto.TemplateId,
            ownerId,
            dto.Values
        );

        // 3. Send through Mediator
        var result = await _mediator.Send(command);

        return Ok(result);
}
*/


