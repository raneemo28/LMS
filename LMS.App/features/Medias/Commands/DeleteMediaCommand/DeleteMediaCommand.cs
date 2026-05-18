using MediatR;

namespace LMS.App.Features.Media.Commands.DeleteMediaCommand;

public record DeleteMediaCommand(int MediaId, string CurrentUserId, bool IsAdmin = false) : IRequest<bool>;