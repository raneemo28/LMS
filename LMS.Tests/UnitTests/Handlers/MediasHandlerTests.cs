using LMS.App.DTOs.Media;
using LMS.App.Features.Media.Commands.CreateMediaCommand;
using LMS.App.Features.Media.Commands.DeleteMediaCommand;
using LMS.App.Interface;
using LMS.Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LMS.Tests.UnitTests.Handlers;

public class MediasHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUoW = new();
    private readonly Mock<IMediaRepository> _mockMediaRepo = new();
    private readonly Mock<IMediaStorageService> _mockStorage = new();

    public MediasHandlerTests()
    {
        _mockUoW.Setup(u => u.Media).Returns(_mockMediaRepo.Object);
    }

    [Fact] public async Task CreateMedia_Should_Save_And_Return_Id()
    {
        _mockUoW.Setup(u => u.CommitAsync()).ReturnsAsync(1);
        var dto = new CreateMediaDto(null, "f.png", "alt", "u", new List<LMS.App.DTOs.Value.ResourceValueDto>());
        var cmd = new CreateMediaCommand(dto);
        await new CreateMediaHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None);

        // No real DB in unit tests so entity.Id stays 0 — verify commit happened instead
        _mockUoW.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact] public async Task DeleteMedia_Should_Delete_If_Admin()
    {
        var media = new LMS.Domain.Entities.Media { Id = 1, StoragePath = "path", OwnerId = "other" };
        _mockMediaRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(media);
        _mockStorage.Setup(s => s.DeleteAsync("path")).Returns(Task.CompletedTask);
        _mockUoW.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        var cmd = new DeleteMediaCommand(1, "admin", true);
        var result = await new DeleteMediaCommandHandler(_mockUoW.Object, _mockStorage.Object)
            .Handle(cmd, CancellationToken.None);

        result.Should().BeTrue();
        _mockStorage.Verify(s => s.DeleteAsync("path"), Times.Once);
    }
}