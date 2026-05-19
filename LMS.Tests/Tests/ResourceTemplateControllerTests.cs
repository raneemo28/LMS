// ============================================================
//  FILE: Tests/ResourceTemplateControllerTests.cs
//
//  Tests for:
//    POST   /api/resourcetemplate                           (auth required)
//    GET    /api/resourcetemplate/{id}
//    PUT    /api/resourcetemplate/{id}
//    DELETE /api/resourcetemplate/{id}
//    POST   /api/resourcetemplate/{id}/properties
//    PUT    /api/resourcetemplate/{id}/properties/{propId}
//    DELETE /api/resourcetemplate/{id}/properties/{propId}
// ============================================================

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LMS.Tests.Infrastructure;
using Xunit;

namespace LMS.Tests.Tests;

public class ResourceTemplateControllerTests : TestFixture
{
    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Creates a template and returns its new ID.</summary>
    private async Task<int> CreateTemplateAsync(string label = "")
    {
        label = string.IsNullOrEmpty(label) ? $"Template_{Guid.NewGuid():N}" : label;
        var response = await Client.PostAsJsonAsync("/api/resourcetemplate", new
        {
            label,
            description = "Created by integration test"
        });
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("id").GetInt32();
    }

    /// <summary>Creates a vocabulary + property, returns the propertyId.</summary>
    private async Task<int> CreatePropertyAsync()
    {
        var vocabResp = await AdminClient.PostAsJsonAsync("/api/vocabularies", new
        {
            prefix       = $"t_{Guid.NewGuid():N}".Substring(0, 8),
            namespaceUri = $"http://tpl.test/{Guid.NewGuid():N}#",
            label        = $"TplVocab_{Guid.NewGuid():N}"
        });
        var vocabBody = await vocabResp.Content.ReadAsStringAsync();
        using var vocabDoc = JsonDocument.Parse(vocabBody);
        var vocabId = vocabDoc.RootElement.GetProperty("id").GetInt32();

        var propResp = await AdminClient.PostAsJsonAsync(
            $"/api/vocabularies/{vocabId}/properties", new
            {
                localName = $"prop_{Guid.NewGuid():N}".Substring(0, 10),
                label     = "Test Prop",
                termUri   = $"http://tpl.test/{Guid.NewGuid():N}#prop"
            });
        var propBody = await propResp.Content.ReadAsStringAsync();
        using var propDoc = JsonDocument.Parse(propBody);
        return propDoc.RootElement.GetProperty("propertyId").GetInt32();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // CREATE TEMPLATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateTemplate_Authenticated_Returns201WithId()
    {
        var response = await Client.PostAsJsonAsync("/api/resourcetemplate", new
        {
            label       = $"My Template {Guid.NewGuid():N}",
            description = "A test resource template"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("id").GetInt32().Should().BeGreaterThan(0);
        doc.RootElement.GetProperty("success").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task CreateTemplate_Anonymous_Returns401()
    {
        var response = await CreateAnonymousClient().PostAsJsonAsync("/api/resourcetemplate", new
        {
            label = "Should fail"
        });
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET TEMPLATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetTemplate_ExistingId_Returns200WithLabel()
    {
        var id = await CreateTemplateAsync("GetMe");

        var response = await Client.GetAsync($"/api/resourcetemplate/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("id").GetInt32().Should().Be(id);
    }

    [Fact]
    public async Task GetTemplate_NonExistentId_Returns404()
    {
        var response = await Client.GetAsync("/api/resourcetemplate/999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // UPDATE TEMPLATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateTemplate_Authenticated_Returns200()
    {
        var id = await CreateTemplateAsync();

        var response = await Client.PutAsJsonAsync($"/api/resourcetemplate/{id}", new
        {
            label       = $"Updated Label {id}",
            description = "Updated description"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("success").GetBoolean().Should().BeTrue();
    }

    // ─────────────────────────────────────────────────────────────────────────
    // DELETE TEMPLATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteTemplate_Authenticated_Returns200()
    {
        var id = await CreateTemplateAsync();

        var response = await Client.DeleteAsync($"/api/resourcetemplate/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // ADD PROPERTY TO TEMPLATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task AddPropertyToTemplate_ValidIds_Returns200()
    {
        var templateId = await CreateTemplateAsync();
        var propertyId = await CreatePropertyAsync();

        var response = await Client.PostAsJsonAsync(
            $"/api/resourcetemplate/{templateId}/properties", new
            {
                properties = new[]
                {
                    new
                    {
                        propertyId,
                        isRequired     = true,
                        displayOrder   = 1,
                        alternateLabel = (string?)null
                    }
                }
            });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // UPDATE PROPERTY IN TEMPLATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdatePropertyInTemplate_ExistingLink_Returns200()
    {
        var templateId = await CreateTemplateAsync();
        var propertyId = await CreatePropertyAsync();

        // Add the property first
        await Client.PostAsJsonAsync(
            $"/api/resourcetemplate/{templateId}/properties", new
            {
                properties = new[]
                {
                    new { propertyId, isRequired = false, displayOrder = 1, alternateLabel = (string?)null }
                }
            });

        // Act – update
        var response = await Client.PutAsJsonAsync(
            $"/api/resourcetemplate/{templateId}/properties/{propertyId}", new
            {
                isRequired     = true,
                displayOrder   = 2,
                alternateLabel = "New Label"
            });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // REMOVE PROPERTY FROM TEMPLATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task RemovePropertyFromTemplate_ExistingLink_Returns200()
    {
        var templateId = await CreateTemplateAsync();
        var propertyId = await CreatePropertyAsync();

        await Client.PostAsJsonAsync(
            $"/api/resourcetemplate/{templateId}/properties", new
            {
                properties = new[]
                {
                    new { propertyId, isRequired = false, displayOrder = 1, alternateLabel = (string?)null }
                }
            });

        var response = await Client.DeleteAsync(
            $"/api/resourcetemplate/{templateId}/properties/{propertyId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
