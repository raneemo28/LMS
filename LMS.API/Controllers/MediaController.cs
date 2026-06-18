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
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;
using LMS.App.Features.Medias.Queries.GetAllMedias;
namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MediaController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    private readonly IStringLocalizer<SharedResource> _sharedLocalizer; // ADDED

    public MediaController(
        IMediator mediator, 
        IStringLocalizer<ErrorMessages> localizer,
        IStringLocalizer<SharedResource> sharedLocalizer) // ADDED
    {
        _mediator = mediator;
        _localizer = localizer;
        _sharedLocalizer = sharedLocalizer; // ADDED
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllMediasQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMedia([FromBody] CreateMediaDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized(_localizer["UserIdNotFoundToken"]);
        var result = await _mediator.Send(new CreateMediaCommand(dto, userId));
        return CreatedAtAction(nameof(GetMediaWithMetadata), new { mediaId = result }, result);
    }

    [HttpPost("upload")]
    [RequestSizeLimit(52428800)]
    public async Task<IActionResult> UploadMedia([FromForm] int mediaId, IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest(_localizer["NoFileUploaded"]);
        using var fileStream = file.OpenReadStream();
        var storagePath = await _mediator.Send(new UploadMediaFileCommand(mediaId, fileStream, file.ContentType, file.Length, file.FileName));
        return Ok(new { path = storagePath });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditMedia(int id, [FromBody] UpdateMediaDto dto)
    {
        if (id != dto.Id) return BadRequest(_localizer["MediaIdMismatch"]);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized(_localizer["UserIdNotFoundToken"]);
        var result = await _mediator.Send(new UpdateMediaCommand(dto, userId));
        return result ? Ok(new { message = _sharedLocalizer["Success"] }) : NotFound(); // CHANGED
    }

    [HttpDelete("{mediaId}")]
    public async Task<IActionResult> DeleteMedia([FromRoute] int mediaId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized(_localizer["UserIdentityNotVerified"]);
        bool isAdmin = User.IsInRole("Admin");
        var result = await _mediator.Send(new DeleteMediaCommand(mediaId, userId, isAdmin));
        return result ? Ok(new { message = _sharedLocalizer["Success"] }) : BadRequest(_localizer["FailedToDeleteMedia"]); // CHANGED
    }

    [HttpGet("item/{itemId:int}")]
    public async Task<IActionResult> GetMediaByItemId(int itemId)
    {
        var result = await _mediator.Send(new GetMediaByItemIdQuery(itemId));
        return result != null ? Ok(result) : NotFound(_localizer["MediaNotFound"]);
    }

    [HttpGet("download/{mediaId:int}")]
    public async Task<IActionResult> DownloadMedia(int mediaId)
    {
        var result = await _mediator.Send(new DownloadMediaCommand(mediaId));
        if (result?.Stream == null) return NotFound(_localizer["MediaNotFound"]);
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