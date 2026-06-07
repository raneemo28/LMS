using LMS.App.DTOs.Value;
using LMS.App.Features.Resources.Commands.AddValues;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Tests.UnitTests.Handlers;

public class ResourcesHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUoW = new();
    private readonly Mock<IResourceRepository<Resource>> _mockResRepo = new();
    private readonly Mock<IVocabularyRepository> _mockVocabRepo = new();

    public ResourcesHandlerTests()
    {
        _mockUoW.Setup(u => u.Resources).Returns(_mockResRepo.Object);
        _mockUoW.Setup(u => u.Vocabularies).Returns(_mockVocabRepo.Object);
    }

    [Fact] public async Task AddValues_Should_Succeed_If_Property_Found()
    {
        _mockVocabRepo.Setup(v => v.GetPropertyByIdAsync(1)).ReturnsAsync(new Property());
        _mockResRepo.Setup(r => r.AddValueAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Value());
        _mockUoW.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        var cmd = new AddValuesCommand(1, new List<CreateResourceValueDto> { new(1, "v", null, null, "text", "en") });
        var result = await new AddValuesHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None);
        result.Should().BeTrue();
    }

    [Fact] public async Task AddValues_Should_Throw_If_Property_Not_Found()
    {
        _mockVocabRepo.Setup(v => v.GetPropertyByIdAsync(1)).ReturnsAsync((Property?)null);
        var cmd = new AddValuesCommand(1, new List<CreateResourceValueDto> { new(1, "v", null, null, "text", "en") });
        await Assert.ThrowsAsync<KeyNotFoundException>(() => new AddValuesHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None));
    }
}