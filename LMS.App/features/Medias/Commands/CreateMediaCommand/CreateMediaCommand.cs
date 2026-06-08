using LMS.App.DTOs.Media;
using MediatR;

namespace LMS.App.Features.Media.Commands.CreateMediaCommand;

public record CreateMediaCommand(
    CreateMediaDto Dto,
    string OwnerId
) : IRequest<int>;