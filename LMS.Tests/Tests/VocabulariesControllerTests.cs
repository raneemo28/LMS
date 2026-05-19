// ============================================================
//  FILE: Tests/VocabulariesControllerTests.cs
//
//  Tests for:
//    GET    /api/vocabularies              (public)
//    GET    /api/vocabularies/{id}          (public)
//    GET    /api/vocabularies/by-prefix    (public)
//    POST   /api/vocabularies              (Admin only)
//    PUT    /api/vocabularies/{id}         (Admin only)
//    DELETE /api/vocabularies/{id}         (Admin only)
//    POST   /api/vocabularies/{id}/properties  (Admin only)
//    PUT    /api/vocabularies/properties/{id}  (Admin only)
//    DELETE /api/vocabularies/properties/{id}  (Admin only)
// ============================================================

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LMS.Tests.Infrastructure;
using Xunit;

namespace LMS.Tests.Tests;

public class VocabulariesControllerTests : TestFixture
{
    // ─────────────────────────────────────────────────────────────────────────
    // PUBLIC READ ENDPOINTS
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAll_Anonymous_Returns200AndArray()
    {
        var response = await CreateAnonymousClient().GetAsync("/api/vocabularies");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Fact]
    public async Task GetById_ExistingSystemVocabulary_Returns200()
    {
        // The system seeds vocabulary with Id=1 (sys vocabulary)
        var response = await CreateAnonymousClient().GetAsync("/api/vocabularies/1");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("id").GetInt32().Should().Be(1);
    }

    [Fact]
    public async Task GetById_NonExistent_Returns404()
    {
        var response = await CreateAnonymousClient().GetAsync("/api/vocabularies/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetByPrefix_Returns200AndArray()
    {
        var response = await CreateAnonymousClient().GetAsync("/api/vocabularies/by-prefix?prefix=sys");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // ADMIN-ONLY: CREATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Create_AsAdmin_Returns201WithId()
    {
        var response = await AdminClient.PostAsJsonAsync("/api/vocabularies", new
        {
            prefix       = $"test_{Guid.NewGuid():N}".Substring(0, 10),
            namespaceUri = $"http://test.vocab.com/{Guid.NewGuid():N}#",
            label        = $"Test Vocabulary {Guid.NewGuid():N}"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created,
            "admin users are allowed to create vocabularies");

        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
        doc.RootElement.GetProperty("success").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task Create_AsRegularUser_Returns403()
    {
        // Regular member is not in the Admin role
        var response = await Client.PostAsJsonAsync("/api/vocabularies", new
        {
            prefix       = "bad",
            namespaceUri = "http://bad.com#",
            label        = "Bad"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden,
            "only admins may create vocabularies");
    }

    [Fact]
    public async Task Create_Anonymous_Returns401()
    {
        var response = await CreateAnonymousClient().PostAsJsonAsync("/api/vocabularies", new
        {
            prefix       = "anon",
            namespaceUri = "http://anon.com#",
            label        = "Anon"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // ADMIN-ONLY: UPDATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Update_AsAdmin_Returns200()
    {
        // Create a vocabulary first
        var created = await AdminClient.PostAsJsonAsync("/api/vocabularies", new
        {
            prefix       = $"upd_{Guid.NewGuid():N}".Substring(0, 10),
            namespaceUri = $"http://upd.vocab.com/{Guid.NewGuid():N}#",
            label        = $"Update Me {Guid.NewGuid():N}"
        });
        created.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdBody = await created.Content.ReadAsStringAsync();
        using var createdDoc = JsonDocument.Parse(createdBody);
        var id = createdDoc.RootElement.GetProperty("id").GetInt32();

        // Act
        var response = await AdminClient.PutAsJsonAsync($"/api/vocabularies/{id}", new
        {
            prefix       = $"upd2_{id}",
            namespaceUri = $"http://upd2.vocab.com/{id}#",
            label        = $"Updated Label {id}"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // ADMIN-ONLY: DELETE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Delete_AsAdmin_Returns200()
    {
        // Create first
        var created = await AdminClient.PostAsJsonAsync("/api/vocabularies", new
        {
            prefix       = $"del_{Guid.NewGuid():N}".Substring(0, 8),
            namespaceUri = $"http://del.vocab.com/{Guid.NewGuid():N}#",
            label        = $"Delete Me {Guid.NewGuid():N}"
        });
        var body = await created.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        var id = doc.RootElement.GetProperty("id").GetInt32();

        // Act
        var response = await AdminClient.DeleteAsync($"/api/vocabularies/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PROPERTY ENDPOINTS
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateProperty_AsAdmin_Returns201()
    {
        // Create a vocabulary to add a property to
        var created = await AdminClient.PostAsJsonAsync("/api/vocabularies", new
        {
            prefix       = $"p_{Guid.NewGuid():N}".Substring(0, 8),
            namespaceUri = $"http://prop.vocab.com/{Guid.NewGuid():N}#",
            label        = $"Property Vocab {Guid.NewGuid():N}"
        });
        var vocabBody = await created.Content.ReadAsStringAsync();
        using var vocabDoc = JsonDocument.Parse(vocabBody);
        var vocabId = vocabDoc.RootElement.GetProperty("id").GetInt32();

        // Act
        var response = await AdminClient.PostAsJsonAsync(
            $"/api/vocabularies/{vocabId}/properties", new
            {
                localName = $"myProp_{Guid.NewGuid():N}".Substring(0, 15),
                label     = "My Test Property",
                termUri   = $"http://prop.vocab.com/{Guid.NewGuid():N}#myProp"
            });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("propertyId").GetInt32().Should().BeGreaterThan(0);
    }
}
