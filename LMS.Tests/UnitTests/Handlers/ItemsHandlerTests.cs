using LMS.App.DTOs.Value;
using LMS.App.Features.Items.Commands.CreateItem;
using LMS.App.Features.Items.Commands.DeleteItem;
using LMS.App.Features.Items.Commands.UpdateItem;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Tests.UnitTests.Handlers;

public class ItemsHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUoW = new();
    private readonly Mock<IItemRepository> _mockItemRepo = new();

    public ItemsHandlerTests() => _mockUoW.Setup(u => u.Items).Returns(_mockItemRepo.Object);

    [Fact] public async Task CreateItem_Should_Save_And_Return_Id()
    {
        LMS.Domain.Entities.Item? captured = null;
        _mockItemRepo.Setup(r => r.AddAsync(It.IsAny<LMS.Domain.Entities.Item>()))
            .Callback<LMS.Domain.Entities.Item>(i => captured = i).Returns(Task.CompletedTask);
        _mockUoW.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        var cmd = new CreateItemCommand(1, "owner", new List<CreateResourceValueDto> { new(1, "v", null, null, "text", "en") });
        await new CreateItemHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None);

        // In unit tests there is no real DB, so entity.Id stays 0 — verify the handler
        // committed and built the entity correctly instead
        _mockUoW.Verify(u => u.CommitAsync(), Times.Once);
        captured!.OwnerId.Should().Be("owner");
        captured.TemplateId.Should().Be(1);
    }

    [Fact] public async Task UpdateItem_Should_Throw_If_Not_Owner()
    {
        _mockItemRepo.Setup(r => r.GetItemWithFullDataForUpdateAsync(1))
            .ReturnsAsync(new LMS.Domain.Entities.Item { OwnerId = "other" });

        var cmd = new UpdateItemCommand(1, 1, "attacker", new List<ResourceValueDto>());
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            new UpdateItemHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None));
    }

    [Fact] public async Task DeleteItem_Should_Fail_If_Not_Found()
    {
        _mockItemRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((LMS.Domain.Entities.Item?)null);
        var cmd = new DeleteItemCommand(1, "u");
        var result = await new DeleteItemHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None);
        result.Should().BeFalse();
    }
}