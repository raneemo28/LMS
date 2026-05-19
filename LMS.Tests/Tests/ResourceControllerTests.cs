// ============================================================
//  FILE: Tests/ResourceControllerTests.cs
//
//  Tests for:
//    POST   /api/resource/{resourceId}/values
//    GET    /api/resource/{resourceId}/values
//    PUT    /api/resource/{resourceId}/values/{valueId}
//    DELETE /api/resource/{resourceId}/values/{valueId}
// ============================================================

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LMS.Tests.Infrastructure;
using Xunit;

namespace LMS.Tests.Tests;

public class ResourceControllerTests : TestFixture
{
    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Creates a vocabulary, a property inside it, and an item set (which is a
    /// Resource), then returns (resourceId, propertyId) so we can add values.
    /// </summary>
    private async Task<(int ResourceId, int PropertyId)> CreateResourceWithPropertyAsync()
    {
        // 1. Create vocabulary
        var vocabResp = await AdminClient.PostAsJsonAsync("/api/vocabularies", new
        {
            prefix       = $"rv_{Guid.NewGuid():N}".Substring(0, 8),
            namespaceUri = $"http://rv.test/{Guid.NewGuid():N}#",
            label        = $"RV Vocab {Guid.NewGuid():N}"
        });
        vocabResp.EnsureSuccessStatusCode();
        var vocabBody = await vocabResp.Content.ReadAsStringAsync();
        using var vocabDoc = JsonDocument.Parse(vocabBody);
        var vocabId = vocabDoc.RootElement.GetProperty("id").GetInt32();

        // 2. Create property inside vocabulary
        var propResp = await AdminClient.PostAsJsonAsync(
            $"/api/vocabularies/{vocabId}/properties", new
            {
                localName = $"rvp_{Guid.NewGuid():N}".Substring(0, 8),
                label     = "RV Property",
                termUri   = $"http://rv.test/{Guid.NewGuid():N}#rvp"
            });
        propResp.EnsureSuccessStatusCode();
        var propBody = await propResp.Content.ReadAsStringAsync();
        using var propDoc = JsonDocument.Parse(propBody);
        var propertyId = propDoc.RootElement.GetProperty("propertyId").GetInt32();

        // 3. Create an ItemSet (it's a Resource, so it can hold values)
        var setResp = await Client.PostAsJsonAsync("/api/itemsets", new
        {
            title    = $"RV Set {Guid.NewGuid():N}",
            isPublic = true,
            values   = (object[]?)null
        });
        setResp.EnsureSuccessStatusCode();
        var resourceId = int.Parse((await setResp.Content.ReadAsStringAsync()).Trim());

        return (resourceId, propertyId);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // ADD VALUES
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task AddValues_ValidRequest_Returns200()
    {
        var (resourceId, propertyId) = await CreateResourceWithPropertyAsync();

        var response = await Client.PostAsJsonAsync(
            $"/api/resource/{resourceId}/values",
            new[]
            {
                new
                {
                    propertyId,
                    valueText       = "hello world",
                    valueResourceId = (int?)null,
                    type            = "text",
                    language        = (string?)null
                }
            });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("success").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task AddValues_Anonymous_Returns401()
    {
        var (resourceId, propertyId) = await CreateResourceWithPropertyAsync();

        var response = await CreateAnonymousClient().PostAsJsonAsync(
            $"/api/resource/{resourceId}/values",
            new[] { new { propertyId, valueText = "x", type = "text" } });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET VALUES
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetResourceValues_Returns200AndArray()
    {
        var (resourceId, propertyId) = await CreateResourceWithPropertyAsync();

        // Add a value first
        await Client.PostAsJsonAsync($"/api/resource/{resourceId}/values",
            new[] { new { propertyId, valueText = "getme", type = "text", language = (string?)null, valueResourceId = (int?)null } });

        var response = await Client.GetAsync($"/api/resource/{resourceId}/values");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // UPDATE VALUE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateValue_ExistingValue_Returns200()
    {
        var (resourceId, propertyId) = await CreateResourceWithPropertyAsync();

        // Add
        await Client.PostAsJsonAsync($"/api/resource/{resourceId}/values",
            new[] { new { propertyId, valueText = "original", type = "text", language = (string?)null, valueResourceId = (int?)null } });

        // Get the value ID
        var getResp = await Client.GetAsync($"/api/resource/{resourceId}/values");
        var getBody = await getResp.Content.ReadAsStringAsync();
        using var getDoc = JsonDocument.Parse(getBody);
        var valueId = getDoc.RootElement[0].GetProperty("id").GetInt32();

        // Act
        var response = await Client.PutAsJsonAsync(
            $"/api/resource/{resourceId}/values/{valueId}", new
            {
                valueText       = "updated",
                valueResourceId = (int?)null,
                type            = "text",
                language        = (string?)null
            });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("success").GetBoolean().Should().BeTrue();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // REMOVE VALUE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task RemoveValue_ExistingValue_Returns200()
    {
        var (resourceId, propertyId) = await CreateResourceWithPropertyAsync();

        // Add
        await Client.PostAsJsonAsync($"/api/resource/{resourceId}/values",
            new[] { new { propertyId, valueText = "delete me", type = "text", language = (string?)null, valueResourceId = (int?)null } });

        // Get the value ID
        var getResp = await Client.GetAsync($"/api/resource/{resourceId}/values");
        var getBody = await getResp.Content.ReadAsStringAsync();
        using var getDoc = JsonDocument.Parse(getBody);
        var valueId = getDoc.RootElement[0].GetProperty("id").GetInt32();

        // Act
        var response = await Client.DeleteAsync(
            $"/api/resource/{resourceId}/values/{valueId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
