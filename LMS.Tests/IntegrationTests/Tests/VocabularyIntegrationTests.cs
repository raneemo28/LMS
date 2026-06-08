using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using LMS.Tests.IntegrationTests.Helpers;
using Xunit;

namespace LMS.Tests.IntegrationTests.Tests;

[Collection("Integration")]
public class VocabularyIntegrationTests : IClassFixture<LmsWebApplicationFactory>
{
    private readonly HttpClient _client;

    public VocabularyIntegrationTests(LmsWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_Vocabularies_Should_Return_200_Without_Auth()
    {
        var response = await _client.GetAsync("/api/vocabularies");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_Vocabulary_Should_Return_401_Without_Token()
    {
        var response = await _client.PostAsJsonAsync("/api/vocabularies", new
        {
            label        = "Dublin Core",
            prefix       = "dc",
            namespaceUri = "http://purl.org/dc/elements/1.1/"
        });

        // Endpoint is [Authorize(Roles = "Admin")] — no token means 401
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Vocabulary_Should_Return_403_For_Non_Admin_User()
    {
        var token = await AuthHelper.RegisterAndLoginAsync(
            _client, $"vocab.member.{Guid.NewGuid()}@lms.com");

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/api/vocabularies", new
        {
            label        = "Test Vocab",
            prefix       = "tv",
            namespaceUri = "http://test.example.com/"
        });

        // Member role is not Admin → 403 Forbidden
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        _client.DefaultRequestHeaders.Authorization = null;
    }
}