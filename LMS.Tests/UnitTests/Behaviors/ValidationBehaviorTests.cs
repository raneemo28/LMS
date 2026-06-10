using FluentValidation;
using FluentValidation.Results;
using LMS.App.Behaviors;
using LMS.App.DTOs.Item;
using LMS.App.DTOs.Value;
using LMS.App.Features.Items.Commands.CreateItem;
using MediatR;
using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Tests.UnitTests.Behaviors;

public class ValidationBehaviorTests
{
    private readonly Mock<IValidator<CreateItemCommand>> _mockValidator;
    private readonly ValidationBehavior<CreateItemCommand, int> _behavior;

    public ValidationBehaviorTests()
    {
        _mockValidator = new Mock<IValidator<CreateItemCommand>>();
        _behavior = new ValidationBehavior<CreateItemCommand, int>(new[] { _mockValidator.Object });
    }

    [Fact]
    public async Task Handle_Should_Call_Next_When_Validation_Passes()
    {
        _mockValidator
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<CreateItemCommand>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        bool nextCalled = false;
        RequestHandlerDelegate<int> next = (ct) =>
        {
            nextCalled = true;
            return Task.FromResult(42);
        };

        var cmd = new CreateItemCommand(
            new CreateItemDto(1, new List<CreateResourceValueDto>()),
            "user-1");

        await _behavior.Handle(cmd, next, CancellationToken.None);

        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Throw_ValidationException_When_Fails()
    {
        var failures = new List<ValidationFailure>
        {
            new("Dto.TemplateId", "TemplateId must be greater than 0.")
        };

        _mockValidator
            .Setup(v => v.ValidateAsync(
                It.IsAny<ValidationContext<CreateItemCommand>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        RequestHandlerDelegate<int> next = (ct) => Task.FromResult(42);

        var cmd = new CreateItemCommand(
            new CreateItemDto(0, new List<CreateResourceValueDto>()),
            "user-1");

        await Assert.ThrowsAsync<ValidationException>(() =>
            _behavior.Handle(cmd, next, CancellationToken.None));
    }
}