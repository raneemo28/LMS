using AutoMapper;
using LMS.App.DTOs.ItemSet;
using LMS.App.Features.ItemSets.Commands.CreateItemSets;
using LMS.App.Features.ItemSets.Commands.DeleteItemSets;
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

public class ItemSetsHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUoW = new();
    private readonly Mock<IItemSetRepository> _mockSetRepo = new();
    private readonly IMapper _mapper;

    public ItemSetsHandlerTests()
    {
        _mockUoW.Setup(u => u.ItemSets).Returns(_mockSetRepo.Object);
        _mapper = new MapperConfiguration(cfg => { cfg.AddProfile(new ItemSetMappingProfile()); }, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance).CreateMapper();
    }

    [Fact]
    public async Task CreateItemSet_Should_Save_And_Return_Id()
    {
        _mockUoW.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        var cmd = new CreateItemSetCommand(
            new CreateItemSetDto("Set", "desc", true, null),
            "owner");

        await new CreateItemSetHandler(_mockUoW.Object, _mapper).Handle(cmd, CancellationToken.None);

        _mockUoW.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteItemSet_Should_Throw_If_Not_Owner()
    {
        _mockSetRepo
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new ItemSet { OwnerId = "owner" });
        _mockSetRepo
            .Setup(r => r.IsOwnerAsync(1, "attacker"))
            .ReturnsAsync(false);

        var cmd = new DeleteItemSetCommand(1, "attacker", new List<string>());

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            new DeleteItemSetHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None));
    }
}
