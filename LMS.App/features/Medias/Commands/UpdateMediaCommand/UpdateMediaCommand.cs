using LMS.App.DTOs.Media;
using LMS.App.DTOs.Value;
using MediatR;

namespace LMS.App.Features.Medias.Commands.UpdateMediaCommand;

public record UpdateMediaCommand(
    UpdateMediaDto Dto
) : IRequest<bool>;
