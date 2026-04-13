using MediatR;

namespace LMS.Application.Features.Resources.Commands.AddResourceWithValue;

public record AddResourceWithValueCommand(
    string ResourceType,
    string? OwnerId,
    int PropertyId,
    string? ValueText,
    string? ValueUri,
    int? ValueResourceId,
    string ValueType,
    string? Language
) : IRequest<int>; // سيعيد الـ Id الخاص بالمورد الجديد