// ============================================================
//  FILE: Tests/ItemSetsControllerTests.cs
//
//  Tests for:
//    POST   /api/itemsets              (auth)
//    GET    /api/itemsets              (public)
//    GET    /api/itemsets/public       (public)
//    GET    /api/itemsets/{id}         (public)
//    GET    /api/itemsets/{id}/ownership (auth)
//    PUT    /api/itemsets/{id}         (auth)
//    DELETE /api/itemsets/{id}         (auth)
//    POST   /api/itemsets/{setId}/items/{itemId}   (auth)
//    DELETE /api/itemsets/{setId}/items/{itemId}   (auth)
// ============================================================

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LMS.Tests.Infrastructure;
using Xunit;

namespace LMS.Tests.Tests;

public class ItemSetsControllerTests : TestFixture
{
    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    private async Task<int> CreateItemSetAsync(bool isPublic = true)
    {
        var resp = await Client.PostAsJsonAsync("/api/itemsets", new
        {
            title       = $"Set_{Guid.NewGuid():N}",
            description = "test set",
            isPublic,
            values      = (object[]?)null
        });
        resp.EnsureSuccessStatusCode();
        var body = await resp.Content.ReadAsStringAsync();
        return int.Parse(body.Trim());
    }

    private async Task<int> CreateItemAsync()
    {
        var tplResp = await Client.PostAsJsonAsync("/api/resourcetemplate", new
        {
            label = $"SetItemTpl_{Guid.NewGuid():N}", description = "auto"
        });
        tplResp.EnsureSuccessStatusCode();
        var tplBody = await tplResp.Content.ReadAsStringAsync();
        using var tplDoc = JsonDocument.Parse(tplBody);
        var templateId = tplDoc.RootElement.GetProperty("id").GetInt32();

        var itemResp = await Client.PostAsJsonAsync("/api/items", new
        {
            templateId, values = Array.Empty<object>()
        });
        itemResp.EnsureSuccessStatusCode();
        return int.Parse((await itemResp.Content.ReadAsStringAsync()).Trim());
    }

    // ─────────────────────────────────────────────────────────────────────────
    // CREATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateItemSet_Authenticated_Returns201()
    {
        var response = await Client.PostAsJsonAsync("/api/itemsets", new
        {
            title       = $"My Set {Guid.NewGuid():N}",
            description = "An integration test item set",
            isPublic    = true,
            values      = (object[]?)null
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadAsStringAsync();
        int.TryParse(body.Trim(), out var id).Should().BeTrue();
        id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateItemSet_Anonymous_Returns401()
    {
        var response = await CreateAnonymousClient().PostAsJsonAsync("/api/itemsets", new
        {
            title    = "Should fail",
            isPublic = true,
            values   = (object[]?)null
        });
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // READ
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAll_Anonymous_Returns200AndArray()
    {
        var response = await CreateAnonymousClient().GetAsync("/api/itemsets");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Fact]
    public async Task GetPublicSets_Anonymous_Returns200()
    {
        var response = await CreateAnonymousClient().GetAsync("/api/itemsets/public");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ExistingSet_Returns200()
    {
        var id = await CreateItemSetAsync();

        var response = await CreateAnonymousClient().GetAsync($"/api/itemsets/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        // Response is ItemSetMembersDto which has setInfo.id
        doc.RootElement.GetProperty("setInfo").GetProperty("id").GetInt32().Should().Be(id);
    }

    [Fact]
    public async Task GetById_NonExistent_Returns404()
    {
        var response = await CreateAnonymousClient().GetAsync("/api/itemsets/999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // OWNERSHIP CHECK
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task CheckOwnership_OwnSet_ReturnsTrue()
    {
        var id = await CreateItemSetAsync();

        var response = await Client.GetAsync($"/api/itemsets/{id}/ownership");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Trim().ToLower().Should().Be("true");
    }

    [Fact]
    public async Task CheckOwnership_Anonymous_Returns401()
    {
        var id = await CreateItemSetAsync();

        var response = await CreateAnonymousClient().GetAsync($"/api/itemsets/{id}/ownership");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // UPDATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateItemSet_ByOwner_Returns204()
    {
        var id = await CreateItemSetAsync();

        var response = await Client.PutAsJsonAsync($"/api/itemsets/{id}", new
        {
            id,
            title       = "Updated Title",
            description = "Updated",
            isPublic    = false,
            values      = (object[]?)null
        });

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateItemSet_WithMismatchedId_Returns400()
    {
        var id = await CreateItemSetAsync();

        var response = await Client.PutAsJsonAsync($"/api/itemsets/{id}", new
        {
            id       = id + 1000,  // mismatch
            title    = "x",
            isPublic = true,
            values   = (object[]?)null
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // DELETE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteItemSet_ByOwner_Returns204()
    {
        var id = await CreateItemSetAsync();

        var response = await Client.DeleteAsync($"/api/itemsets/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteItemSet_Anonymous_Returns401()
    {
        var id = await CreateItemSetAsync();

        var response = await CreateAnonymousClient().DeleteAsync($"/api/itemsets/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // ADD / REMOVE ITEMS
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task AddItemToSet_ValidIds_Returns200()
    {
        var setId  = await CreateItemSetAsync();
        var itemId = await CreateItemAsync();

        var response = await Client.PostAsync($"/api/itemsets/{setId}/items/{itemId}", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RemoveItemFromSet_ExistingMembership_Returns204()
    {
        var setId  = await CreateItemSetAsync();
        var itemId = await CreateItemAsync();

        // Add first
        await Client.PostAsync($"/api/itemsets/{setId}/items/{itemId}", null);

        // Now remove
        var response = await Client.DeleteAsync($"/api/itemsets/{setId}/items/{itemId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
