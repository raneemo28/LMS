// ============================================================
//  FILE: Tests/ItemsControllerTests.cs
//
//  Tests for:
//    POST   /api/items        (auth required)
//    GET    /api/items/{id}   (public)
//    GET    /api/items        (public)
//    PUT    /api/items/{id}   (auth required)
//    DELETE /api/items/{id}   (auth required)
// ============================================================

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LMS.Tests.Infrastructure;
using Xunit;

namespace LMS.Tests.Tests;

public class ItemsControllerTests : TestFixture
{
    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Creates a resource template (needed as templateId when creating an item).
    /// Returns the new templateId.
    /// </summary>
    private async Task<int> CreateTemplateAsync()
    {
        var resp = await Client.PostAsJsonAsync("/api/resourcetemplate", new
        {
            label       = $"ItemTemplate_{Guid.NewGuid():N}",
            description = "auto"
        });
        resp.EnsureSuccessStatusCode();
        var body = await resp.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("id").GetInt32();
    }

    /// <summary>Creates an item and returns its new ID.</summary>
    private async Task<int> CreateItemAsync(int templateId)
    {
        var resp = await Client.PostAsJsonAsync("/api/items", new
        {
            templateId,
            values = Array.Empty<object>()   // no values – template has no required props yet
        });
        resp.EnsureSuccessStatusCode();
        var body = await resp.Content.ReadAsStringAsync();
        // CreatedAtAction returns the new ID directly as the body
        return int.Parse(body.Trim());
    }

    // ─────────────────────────────────────────────────────────────────────────
    // CREATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateItem_Authenticated_Returns201WithId()
    {
        var templateId = await CreateTemplateAsync();

        var response = await Client.PostAsJsonAsync("/api/items", new
        {
            templateId,
            values = Array.Empty<object>()
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created,
            "authenticated users can create items");

        var body = await response.Content.ReadAsStringAsync();
        int.TryParse(body.Trim(), out var id).Should().BeTrue(
            $"body should be the new item ID but was: {body}");
        id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateItem_Anonymous_Returns401()
    {
        var templateId = await CreateTemplateAsync();

        var response = await CreateAnonymousClient().PostAsJsonAsync("/api/items", new
        {
            templateId,
            values = Array.Empty<object>()
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // READ
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetById_ExistingItem_Returns200WithCorrectId()
    {
        var templateId = await CreateTemplateAsync();
        var itemId     = await CreateItemAsync(templateId);

        var response = await CreateAnonymousClient().GetAsync($"/api/items/{itemId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("id").GetInt32().Should().Be(itemId);
    }

    [Fact]
    public async Task GetById_NonExistentItem_Returns404()
    {
        var response = await CreateAnonymousClient().GetAsync("/api/items/999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_Anonymous_Returns200AndArray()
    {
        var response = await CreateAnonymousClient().GetAsync("/api/items");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Fact]
    public async Task GetAll_FilterByTemplateId_Returns200()
    {
        var templateId = await CreateTemplateAsync();
        await CreateItemAsync(templateId);

        var response = await CreateAnonymousClient()
            .GetAsync($"/api/items?templateId={templateId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // UPDATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateItem_Authenticated_Returns204()
    {
        var templateId = await CreateTemplateAsync();
        var itemId     = await CreateItemAsync(templateId);

        var response = await Client.PutAsJsonAsync($"/api/items/{itemId}", new
        {
            id         = itemId,
            templateId,
            values     = Array.Empty<object>()
        });

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateItem_WithMismatchedId_Returns400()
    {
        var templateId = await CreateTemplateAsync();
        var itemId     = await CreateItemAsync(templateId);

        // URL id vs body id mismatch
        var response = await Client.PutAsJsonAsync($"/api/items/{itemId}", new
        {
            id         = itemId + 999,  // wrong
            templateId,
            values     = Array.Empty<object>()
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateItem_Anonymous_Returns401()
    {
        var templateId = await CreateTemplateAsync();
        var itemId     = await CreateItemAsync(templateId);

        var response = await CreateAnonymousClient().PutAsJsonAsync($"/api/items/{itemId}", new
        {
            id = itemId, templateId, values = Array.Empty<object>()
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // DELETE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteItem_Authenticated_Returns204()
    {
        var templateId = await CreateTemplateAsync();
        var itemId     = await CreateItemAsync(templateId);

        var response = await Client.DeleteAsync($"/api/items/{itemId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteItem_NonExistent_Returns404()
    {
        var response = await Client.DeleteAsync("/api/items/999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteItem_Anonymous_Returns401()
    {
        var templateId = await CreateTemplateAsync();
        var itemId     = await CreateItemAsync(templateId);

        var response = await CreateAnonymousClient().DeleteAsync($"/api/items/{itemId}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
