using AutoMapper;
using LMS.App.DTOs.Item;
using LMS.App.DTOs.Value;
using LMS.App.Features.Items.Commands.CreateItem;
using LMS.App.Features.Items.Commands.DeleteItem;
using LMS.App.Features.Items.Commands.UpdateItem;
using LMS.App.Profiles;
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
    private readonly IMapper _mapper;

    public ItemsHandlerTests()
    {
        _mockUoW.Setup(u => u.Items).Returns(_mockItemRepo.Object);
        _mapper = new MapperConfiguration(cfg => {
            cfg.AddProfile(new ItemMappingProfile());
            cfg.AddProfile(new ResourceValueMappingProfile());
        }, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance).CreateMapper();
    }

    [Fact]
    public async Task CreateItem_Should_Save_And_Return_Id()
    {
        Item? captured = null;
        _mockItemRepo
            .Setup(r => r.AddAsync(It.IsAny<Item>()))
            .Callback<Item>(i => captured = i)
            .Returns(Task.CompletedTask);
        _mockUoW.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        var cmd = new CreateItemCommand(
            new CreateItemDto(1, new List<CreateResourceValueDto>
            {
                new(1, "v", null, null, "text", "en")
            }),
            "owner");

        await new CreateItemHandler(_mockUoW.Object, _mapper).Handle(cmd, CancellationToken.None);

        _mockUoW.Verify(u => u.CommitAsync(), Times.Once);
        captured!.OwnerId.Should().Be("owner");
        captured.TemplateId.Should().Be(1);
    }

    [Fact]
    public async Task UpdateItem_Should_Throw_If_Not_Owner()
    {
        _mockItemRepo
            .Setup(r => r.GetItemWithFullDataForUpdateAsync(1))
            .ReturnsAsync(new Item { OwnerId = "other" });

        var cmd = new UpdateItemCommand(
            new UpdateItemDto(1, 1, new List<ResourceValueDto>()),
            "attacker");

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            new UpdateItemHandler(_mockUoW.Object, _mapper).Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteItem_Should_Fail_If_Not_Found()
    {
        _mockItemRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Item?)null);

        var result = await new DeleteItemHandler(_mockUoW.Object)
            .Handle(new DeleteItemCommand(1, "u"), CancellationToken.None);

        result.Should().BeFalse();
    }
}
