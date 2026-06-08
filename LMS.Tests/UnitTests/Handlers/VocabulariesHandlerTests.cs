using AutoMapper;
using LMS.App.DTOs.Property;
using LMS.App.Features.Vocabularies.Commands.CreateProperty;
using LMS.App.Features.Vocabularies.Commands.DeleteProperty;
using LMS.App.Profiles;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Tests.UnitTests.Handlers;

public class VocabulariesHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUoW = new();
    private readonly Mock<IVocabularyRepository> _mockVocabRepo = new();
    private readonly IMapper _mapper;

    public VocabulariesHandlerTests()
    {
        _mockUoW.Setup(u => u.Vocabularies).Returns(_mockVocabRepo.Object);
        _mapper = new MapperConfiguration(cfg => { cfg.AddProfile(new VocabularyMappingProfile()); }, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance).CreateMapper();
    }

    [Fact]
    public async Task CreateProperty_Should_Throw_If_Vocab_Not_Found()
    {
        _mockVocabRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Vocabulary?)null);

        var cmd = new CreatePropertyCommand(1, new CreatePropertyDto(1, "name", "Label", "http://u"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            new CreatePropertyHandler(_mockUoW.Object, _mapper).Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteProperty_Should_Throw_If_Linked_Values()
    {
        _mockVocabRepo.Setup(r => r.HasLinkedValuesAsync(1)).ReturnsAsync(true);

        var cmd = new DeletePropertyCommand(1);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new DeletePropertyHandler(_mockUoW.Object).Handle(cmd, CancellationToken.None));
    }
}
