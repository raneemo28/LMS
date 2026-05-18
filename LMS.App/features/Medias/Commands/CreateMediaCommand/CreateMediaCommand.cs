using MediatR;
using LMS.Domain.Constants;
using LMS.App.DTOs.Value;
using LMS.App.DTOs.Media;

namespace LMS.App.Features.Media.Commands.CreateMediaCommand;
public record CreateMediaCommand(
    CreateMediaDto Dto
) : IRequest<int>;
