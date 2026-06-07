using LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;
using LMS.App.Features.ResourceTemplates.Commands.UpdateResourceTemplate;
using LMS.Domain.Interfaces;
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using LMS.Domain.Entities;

namespace LMS.Tests.UnitTests.Handlers;

public class TemplatesHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUoW = new();
    private readonly Mock<IResourceTemplateRepository> _mockTplRepo = new();

    public TemplatesHandlerTests() => _mockUoW.Setup(u => u.ResourceTemplates).Returns(_mockTplRepo.Object);

    [Fact] public async Task CreateTemplate_Should_Throw_If_Label_Exists()
    {
        _mockTplRepo.Setup(r => r.IsLabelUniqueAsync("Dup")).ReturnsAsync(false);
        var cmd = new CreateResourceTemplateCommand("Dup", "desc");
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new CreateResourceTemplateCommandHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None));
    }

    [Fact] public async Task CreateTemplate_Should_Save_If_Unique()
    {
        _mockTplRepo.Setup(r => r.IsLabelUniqueAsync("New")).ReturnsAsync(true);
        _mockUoW.Setup(u => u.CommitAsync()).ReturnsAsync(1);
        var cmd = new CreateResourceTemplateCommand("New", "desc");
        await new CreateResourceTemplateCommandHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None);

        // No real DB in unit tests so entity.Id stays 0 — verify commit happened instead
        _mockUoW.Verify(u => u.CommitAsync(), Times.Once);
    }
}