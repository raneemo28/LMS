using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using LMS.Tests.IntegrationTests.Helpers;
using Xunit;

namespace LMS.Tests.IntegrationTests.Tests;

[Collection("Integration")]
public class ItemSetsIntegrationTests : IClassFixture<LmsWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ItemSetsIntegrationTests(LmsWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ItemSets_Should_Return_200_Without_Auth()
    {
        var response = await _client.GetAsync("/api/itemsets");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_ItemSet_Should_Return_401_Without_Token()
    {
        var response = await _client.PostAsJsonAsync("/api/itemsets", new
        {
            title       = "Test Set",
            description = "desc",
            isPublic    = true
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_ItemSet_Should_Return_400_When_Title_Empty()
    {
        var token = await AuthHelper.RegisterAndLoginAsync(
            _client, $"itemset.tester.{Guid.NewGuid()}@lms.com");

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/api/itemsets", new
        {
            title       = "",      // invalid — validator fires → 400
            description = "desc",
            isPublic    = false
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        _client.DefaultRequestHeaders.Authorization = null;
    }
}