using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LMS.Tests.IntegrationTests.Tests;

[Collection("Integration")]
public class ResourceTemplateIntegrationTests : IClassFixture<LmsWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ResourceTemplateIntegrationTests(LmsWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_Template_Should_Return_401_Without_Token()
    {
        var response = await _client.PostAsJsonAsync("/api/resourcetemplate", new
        {
            label       = "Book",
            description = "A book template"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_Template_Should_Return_404_If_Not_Found()
    {
        var response = await _client.GetAsync("/api/resourcetemplate/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}