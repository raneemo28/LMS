using MediatR;
using LMS.Domain.Entities;

namespace LMS.Application.Features.Resources.Commands.AddValue;

public record AddValueCommand(
    int ResourceId, 
    int PropertyId, 
    string? ValueText, 
    string? ValueUri, 
    int? ValueResourceId, 
    string Type, 
    string? Language
) : IRequest<bool>;
