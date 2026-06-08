using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using LMS.Tests.IntegrationTests.Helpers;
using Xunit;

namespace LMS.Tests.IntegrationTests.Tests;

[Collection("Integration")]
public class ItemsIntegrationTests : IClassFixture<LmsWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ItemsIntegrationTests(LmsWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_Items_Should_Return_200_Without_Auth()
    {
        var response = await _client.GetAsync("/api/items");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_Item_Should_Return_401_Without_Token()
    {
        var response = await _client.PostAsJsonAsync("/api/items", new
        {
            templateId = 1,
            values     = new object[] { }
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Item_Should_Return_400_When_TemplateId_Is_Zero()
    {
        var token = await AuthHelper.RegisterAndLoginAsync(
            _client, $"item.tester.{Guid.NewGuid()}@lms.com");

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/api/items", new
        {
            templateId = 0,        // invalid — ValidationBehavior fires → 400
            values     = new object[] { }
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        _client.DefaultRequestHeaders.Authorization = null;
    }

    [Fact]
    public async Task GetItem_By_Id_Should_Return_404_When_Not_Found()
    {
        var response = await _client.GetAsync("/api/items/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}