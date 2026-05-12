using MediatR;
using LMS.Domain.Entities;

namespace LMS.App.Features.Resources.Commands.AddValue;

public record AddValueCommand(
    int ResourceId,
    int PropertyId,
    string? ValueText,
    int? ValueResourceId,
    string Type,
    string? Language
) : IRequest<bool>;
