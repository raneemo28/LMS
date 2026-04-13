using MediatR;
using LMS.Domain.Constants;

namespace LMS.App.Features.Media.Commands.CreateMediaCommand;

public record CreateMediaCommand(
    string? AltText,
    int? ItemId,
    string OwnerId
) : IRequest<int>;