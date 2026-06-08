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

    [HttpPost]
    public async Task<IActionResult> CreateMedia([FromBody] CreateMediaDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var result = await _mediator.Send(new CreateMediaCommand(dto, userId));
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
            mediaId, fileStream, file.ContentType, file.Length, file.FileName));

        return Ok(new { path = storagePath });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditMedia(int id, [FromBody] UpdateMediaDto dto)
    {
        if (id != dto.Id)
            return BadRequest("Media ID in URL does not match ID in body.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token.");

        var result = await _mediator.Send(new UpdateMediaCommand(dto, userId));
        return result
            ? Ok(new { message = "Media updated successfully." })
            : NotFound();
    }

    [HttpDelete("{mediaId}")]
    public async Task<IActionResult> DeleteMedia([FromRoute] int mediaId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User identity could not be verified.");

        bool isAdmin = User.IsInRole("Admin");
        var result = await _mediator.Send(new DeleteMediaCommand(mediaId, userId, isAdmin));
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

    [HttpGet("download/{mediaId:int}")]
    public async Task<IActionResult> DownloadMedia(int mediaId)
    {
        var result = await _mediator.Send(new DownloadMediaCommand(mediaId));
        if (result?.Stream == null)
            return NotFound($"Media with ID {mediaId} was not found.");
        return File(result.Stream, result.ContentType, result.FileName);
    }

    [HttpGet("by-mimetype")]
    public async Task<IActionResult> GetMediaByMimeType([FromQuery] string mimetype)
    {
        var result = await _mediator.Send(new GetMediaByMimeTypeQuery(mimetype));
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("by-owner/{ownerId}")]
    public async Task<IActionResult> GetMediaByOwner(string ownerId)
    {
        var result = await _mediator.Send(new GetMediaByOwnerQuery(ownerId));
        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("metadata/{mediaId:int}")]
    public async Task<IActionResult> GetMediaWithMetadata(int mediaId)
    {
        var result = await _mediator.Send(new GetMediaWithMetadataQuery(mediaId));
        return Ok(result);
    }
}