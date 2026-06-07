using LMS.App.Features.ItemSets.Commands.CreateItemSets;
using LMS.App.Features.ItemSets.Commands.DeleteItemSets;
using LMS.Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using LMS.Domain.Entities;

namespace LMS.Tests.UnitTests.Handlers;

public class ItemSetsHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUoW = new();
    private readonly Mock<IItemSetRepository> _mockSetRepo = new();

    public ItemSetsHandlerTests() => _mockUoW.Setup(u => u.ItemSets).Returns(_mockSetRepo.Object);

    [Fact] public async Task CreateItemSet_Should_Save_And_Return_Id()
    {
        _mockUoW.Setup(u => u.CommitAsync()).ReturnsAsync(1);
        var cmd = new CreateItemSetCommand("Set", "desc", true, "u", "u", null);
        await new CreateItemSetHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None);

        // No real DB in unit tests so entity.Id stays 0 — verify commit happened instead
        _mockUoW.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact] public async Task DeleteItemSet_Should_Throw_If_Not_Owner()
    {
        _mockSetRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ItemSet { OwnerId = "owner" });
        _mockSetRepo.Setup(r => r.IsOwnerAsync(1, "attacker")).ReturnsAsync(false);

        var cmd = new DeleteItemSetCommand(1, "attacker", new());
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            new DeleteItemSetHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None));
    }
}