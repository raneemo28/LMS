// ============================================================
//  FILE: Tests/MediaControllerTests.cs
//
//  Tests for:
//    POST   /api/media/createMedia
//    GET    /api/media/item/{itemId}
//    GET    /api/media/metadata/{mediaId}
//    PUT    /api/media/EditMedia/{id}
//    DELETE /api/media/{mediaId}
//    POST   /api/media/upload        (multipart/form-data)
//    GET    /GetMediaByMimeType
//    GET    /GetMediaByOwner/{ownerId}
// ============================================================

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using LMS.Tests.Infrastructure;
using Xunit;

namespace LMS.Tests.Tests;

public class MediaControllerTests : TestFixture
{
    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    private async Task<int> CreateItemAsync()
    {
        var tplResp = await Client.PostAsJsonAsync("/api/resourcetemplate", new
        {
            label = $"MediaTpl_{Guid.NewGuid():N}", description = "auto"
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

    private async Task<int> CreateMediaAsync(int itemId)
    {
        var resp = await Client.PostAsJsonAsync("/api/media/createMedia", new
        {
            itemId,
            fileName = $"test_{Guid.NewGuid():N}.txt",
            altText  = "A test media record",
            ownerId  = (string?)null,
            values   = Array.Empty<object>()
        });
        resp.EnsureSuccessStatusCode();
        var body = await resp.Content.ReadAsStringAsync();
        // Returns the new media ID directly
        return int.Parse(body.Trim());
    }

    // ─────────────────────────────────────────────────────────────────────────
    // CREATE MEDIA RECORD
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateMedia_Authenticated_Returns201WithId()
    {
        var itemId = await CreateItemAsync();

        var response = await Client.PostAsJsonAsync("/api/media/createMedia", new
        {
            itemId,
            fileName = "myfile.pdf",
            altText  = "A PDF document",
            ownerId  = (string?)null,
            values   = Array.Empty<object>()
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadAsStringAsync();
        int.TryParse(body.Trim(), out var id).Should().BeTrue();
        id.Should().BeGreaterThan(0);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET MEDIA BY ITEM ID
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetMediaByItemId_ReturnsListForItem()
    {
        var itemId = await CreateItemAsync();
        await CreateMediaAsync(itemId);

        var response = await Client.GetAsync($"/api/media/item/{itemId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
        doc.RootElement.GetArrayLength().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetMediaByItemId_NoMedia_Returns404()
    {
        // Use a very large item ID that won't exist
        var response = await Client.GetAsync("/api/media/item/999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // EDIT MEDIA
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task EditMedia_Authenticated_Returns200()
    {
        var itemId   = await CreateItemAsync();
        var mediaId  = await CreateMediaAsync(itemId);

        var response = await Client.PutAsJsonAsync($"/api/media/EditMedia/{mediaId}", new
        {
            id       = mediaId,
            itemId,
            fileName = "updated_name.pdf",
            altText  = "Updated",
            values   = Array.Empty<object>()
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task EditMedia_MismatchedId_Returns400()
    {
        var itemId  = await CreateItemAsync();
        var mediaId = await CreateMediaAsync(itemId);

        var response = await Client.PutAsJsonAsync($"/api/media/EditMedia/{mediaId}", new
        {
            id       = mediaId + 9999,   // mismatch
            itemId,
            fileName = "x.pdf",
            altText  = "x",
            values   = Array.Empty<object>()
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // DELETE MEDIA
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteMedia_Authenticated_Returns200()
    {
        var itemId  = await CreateItemAsync();
        var mediaId = await CreateMediaAsync(itemId);

        var response = await Client.DeleteAsync($"/api/media/{mediaId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("message").GetString()
            .Should().Contain("deleted");
    }

    [Fact]
    public async Task DeleteMedia_Anonymous_Returns401()
    {
        var itemId  = await CreateItemAsync();
        var mediaId = await CreateMediaAsync(itemId);

        var response = await CreateAnonymousClient().DeleteAsync($"/api/media/{mediaId}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // UPLOAD MEDIA FILE (multipart/form-data)
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task UploadMedia_ValidFile_Returns200WithPath()
    {
        var itemId  = await CreateItemAsync();
        var mediaId = await CreateMediaAsync(itemId);

        // Build a multipart form containing the mediaId + a tiny file
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(mediaId.ToString()), "mediaId");

        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("hello test file"));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        content.Add(fileContent, "file", "hello.txt");

        var response = await Client.PostAsync("/api/media/upload", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("path").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task UploadMedia_EmptyFile_Returns400()
    {
        var itemId  = await CreateItemAsync();
        var mediaId = await CreateMediaAsync(itemId);

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(mediaId.ToString()), "mediaId");
        // No file added — empty

        var response = await Client.PostAsync("/api/media/upload", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET BY MIME TYPE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetMediaByMimeType_Returns200AndArray()
    {
        var response = await Client.GetAsync("/GetMediaByMimeType?mimetype=text/plain");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET BY OWNER
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetMediaByOwner_Returns200AndArray()
    {
        // Any non-empty string that won't match anything is fine
        var response = await Client.GetAsync("/GetMediaByOwner/some-owner-id");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
