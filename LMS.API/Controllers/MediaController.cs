using LMS.App.Features.Media.Commands.CreateMediaCommand;
using LMS.App.Features.Media.Commands.DeleteMediaCommand;
using LMS.App.Features.Media.Commands.UploadMediaFile;
using LMS.App.Features.Media.Queries.GetMediaByItemId;
using LMS.App.Features.Medias.Commands.UpdateMediaCommand;
using LMS.App.Features.Medias.Commands.DownloadMedia;
using LMS.App.features.Medias.Queries.GetMediaByMimeType;
using LMS.App.features.Medias.Queries.GetMediaByOwner;
using LMS.App.Features.Medias.Queries.GetMediaWithMetadata;
using LMS.App.DTOs.Media;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MediaController : ControllerBase
{
    private readonly IMediator _mediator;

    public MediaController(IMediator mediator) => _mediator = mediator;


    [HttpPost("createMedia")]
    public async Task<IActionResult> CreateMedia([FromBody] CreateMediaDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var updatedDto = userId != null ? dto with { OwnerId = userId } : dto;

        var result = await _mediator.Send(new CreateMediaCommand(updatedDto));
        return CreatedAtAction(nameof(GetMediaWithMetadata), new { mediaId = result }, result);
    }

    [HttpPost("upload")]
    [RequestSizeLimit(52428800)]
    public async Task<IActionResult> UploadMedia([FromForm] int mediaId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var fileStream = file.OpenReadStream();

        var storagePath = await _mediator.Send(new UploadMediaFileCommand(
            mediaId,
            fileStream,
            file.ContentType,
            file.Length,
            file.FileName
        ));

        return Ok(new { path = storagePath });
    }

    [HttpPut("EditMedia/{id}")]
    public async Task<IActionResult> EditMedia(int id, [FromBody] UpdateMediaDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var updatedDto = dto with { CurrentUserId = userId };

        if (id != updatedDto.Id)
        {
            return BadRequest("media ID in URL does not match ID in body.");
        }

        var command = new UpdateMediaCommand(updatedDto);

        var result = await _mediator.Send(command);
        return Ok(new { message = "Media updated successfully." });
    }

    [HttpDelete("{mediaId}")]
    public async Task<IActionResult> DeleteMedia([FromRoute] int mediaId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized("User identity could not be verified.");

        bool isAdmin = User.IsInRole("Admin");

        var command = new DeleteMediaCommand(mediaId, currentUserId, isAdmin);

        bool result = await _mediator.Send(command);

        return result
            ? Ok(new { message = "Media deleted successfully." })
            : BadRequest("Failed to delete media.");
    }

    [HttpGet("item/{itemId:int}")]
    public async Task<IActionResult> GetMediaByItemId(int itemId)
    {
        var result = await _mediator.Send(new GetMediaByItemIdQuery(itemId));
        return result != null ? Ok(result) : NotFound($"No media found for item {itemId}.");
    }

    [HttpGet("/DownloadMedia/{mediaId:int}")]
    public async Task<IActionResult> DownloadMedia(int mediaId)
    {
        var result = await _mediator.Send(new DownloadMediaCommand(mediaId));

        if (result?.Stream == null)
            return NotFound($"Media with ID {mediaId} was not found.");

        return File(result.Stream, result.ContentType, result.FileName);
    }

    [HttpGet("/GetMediaByMimeType")]
    public async Task<IActionResult> GetMediaByMimeType([FromQuery] string mimetype)
    {
        var result = await _mediator.Send(new GetMediaByMimeTypeQuery(mimetype));
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("/GetMediaByOwner/{ownerId}")]
    public async Task<IActionResult> GetMediaByOwner(string ownerId)
    {
        var result = await _mediator.Send(new GetMediaByOwnerQuery(ownerId));
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("metadata/{mediaId:int}")]
    public async Task<IActionResult> GetMediaWithMetadata(int mediaId)
    {
        try
        {
            var result = await _mediator.Send(new GetMediaWithMetadataQuery(mediaId));
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
    }
}