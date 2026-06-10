using LMS.App.DTOs.Media;
using MediatR;

namespace LMS.App.Features.Medias.Commands.UpdateMediaCommand;

public record UpdateMediaCommand(
    UpdateMediaDto Dto,
    string CurrentUserId
) : IRequest<bool>;