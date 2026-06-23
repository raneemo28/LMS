using AutoMapper;
using LMS.App.DTOs.ResourceTemplate;
using LMS.App.Features.ResourceTemplates.Commands.CreateResourceTemplate;
using LMS.App.Profiles;
using LMS.Domain.Interfaces;
using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using LMS.Tests.TestHelpers;

namespace LMS.Tests.UnitTests.Handlers;

public class TemplatesHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUoW = new();
    private readonly Mock<IResourceTemplateRepository> _mockTplRepo = new();
    private readonly IMapper _mapper;

    public TemplatesHandlerTests()
    {
        _mockUoW.Setup(u => u.ResourceTemplates).Returns(_mockTplRepo.Object);
        _mapper = new MapperConfiguration(cfg => { cfg.AddProfile(new ResourceTemplateMappingProfile()); }, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance).CreateMapper();
    }

    [Fact]
    public async Task CreateTemplate_Should_Throw_If_Label_Exists()
    {
        _mockTplRepo.Setup(r => r.IsLabelUniqueAsync("Dup")).ReturnsAsync(false);

        var cmd = new CreateResourceTemplateCommand(
            new CreateResourceTemplateDto("Dup", "desc"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new CreateResourceTemplateCommandHandler(_mockUoW.Object, _mapper, TestLocalizer.Localizer())
                .Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task CreateTemplate_Should_Save_If_Unique()
    {
        _mockTplRepo.Setup(r => r.IsLabelUniqueAsync("New")).ReturnsAsync(true);
        _mockUoW.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        var cmd = new CreateResourceTemplateCommand(
            new CreateResourceTemplateDto("New", "desc"));

        await new CreateResourceTemplateCommandHandler(_mockUoW.Object, _mapper, TestLocalizer.Localizer())
            .Handle(cmd, CancellationToken.None);

        _mockUoW.Verify(u => u.CommitAsync(), Times.Once);
    }
}
