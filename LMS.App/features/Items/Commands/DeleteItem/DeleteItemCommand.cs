using MediatR;

namespace LMS.App.Features.Items.Commands.DeleteItem;

public record DeleteItemCommand(int Id, string UserId) : IRequest<bool>;

/*
[HttpDelete("{id}")] // The 'id' comes from the URL
public async Task<IActionResult> Delete(int id)
{
    // 1. Get the UserId from the authenticated user's claims
    // This is secure because it comes from the decrypted JWT token
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (userId == null) return Unauthorized();

    // 2. Combine the Route ID and the Secure User ID into the Command
    var command = new DeleteItemCommand(id, userId);

    // 3. Send it to MediatR
    var success = await _mediator.Send(command);

    return success ? NoContent() : NotFound();
}

*/
