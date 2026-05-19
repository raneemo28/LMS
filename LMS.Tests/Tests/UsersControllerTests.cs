// ============================================================
//  FILE: Tests/UsersControllerTests.cs
//
//  Tests for:
//    GET  /api/users                          (Admin only)
//    GET  /api/users/by-role?role=Member      (Admin only)
//    GET  /api/users/{id}                     (Admin only)
//    POST /api/users/{id}/promote-librarian   (Admin only)
//    POST /api/users/{id}/demote-member       (Admin only)
//    POST /api/users/{id}/deactivate          (Admin only)
//    POST /api/users/{id}/reactivate          (Admin only)
// ============================================================

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LMS.Tests.Infrastructure;
using Xunit;

namespace LMS.Tests.Tests;

public class UsersControllerTests : TestFixture
{
    // ─────────────────────────────────────────────────────────────────────────
    // GET ALL USERS
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllUsers_AsAdmin_Returns200AndArray()
    {
        var response = await AdminClient.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
        doc.RootElement.GetArrayLength().Should().BeGreaterThan(0,
            "at least the admin user and the test member exist");
    }

    [Fact]
    public async Task GetAllUsers_AsRegularUser_Returns403()
    {
        var response = await Client.GetAsync("/api/users");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetAllUsers_Anonymous_Returns401()
    {
        var response = await CreateAnonymousClient().GetAsync("/api/users");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET BY ROLE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByRole_Admin_Returns200()
    {
        var response = await AdminClient.GetAsync("/api/users/by-role?role=Admin");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Fact]
    public async Task GetByRole_Member_Returns200()
    {
        var response = await AdminClient.GetAsync("/api/users/by-role?role=Member");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GET BY ID
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetById_AsAdmin_Returns200()
    {
        // First get all users to find a valid ID
        var allResp = await AdminClient.GetAsync("/api/users");
        var allBody = await allResp.Content.ReadAsStringAsync();
        using var allDoc = JsonDocument.Parse(allBody);
        var userId = allDoc.RootElement[0].GetProperty("id").GetString();

        var response = await AdminClient.GetAsync($"/api/users/{userId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("id").GetString().Should().Be(userId);
    }

    [Fact]
    public async Task GetById_NonExistent_Returns404()
    {
        var response = await AdminClient.GetAsync("/api/users/nonexistent-user-id-12345");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PROMOTE / DEMOTE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task PromoteToLibrarian_ThenDemoteToMember_Works()
    {
        // Register a fresh member to promote
        var email    = $"promote_{Guid.NewGuid():N}@test.com";
        var password = "Test@12345";
        await EnsureUserRegistered(email, password, "Promo", "Test");

        // Find their user ID
        var membersResp = await AdminClient.GetAsync("/api/users/by-role?role=Member");
        var membersBody = await membersResp.Content.ReadAsStringAsync();
        using var membersDoc = JsonDocument.Parse(membersBody);

        string? userId = null;
        foreach (var user in membersDoc.RootElement.EnumerateArray())
        {
            if (user.GetProperty("email").GetString() == email)
            {
                userId = user.GetProperty("id").GetString();
                break;
            }
        }
        userId.Should().NotBeNullOrEmpty("the registered user must be findable");

        // Promote to Librarian
        var promoteResp = await AdminClient.PostAsync(
            $"/api/users/{userId}/promote-librarian", null);
        promoteResp.StatusCode.Should().Be(HttpStatusCode.OK);

        // Demote back to Member
        var demoteResp = await AdminClient.PostAsync(
            $"/api/users/{userId}/demote-member", null);
        demoteResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // DEACTIVATE / REACTIVATE
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task DeactivateThenReactivate_Works()
    {
        // Register a fresh user
        var email    = $"deact_{Guid.NewGuid():N}@test.com";
        var password = "Test@12345";
        await EnsureUserRegistered(email, password, "Deact", "Test");

        // Find their ID
        var membersResp = await AdminClient.GetAsync("/api/users/by-role?role=Member");
        var membersBody = await membersResp.Content.ReadAsStringAsync();
        using var membersDoc = JsonDocument.Parse(membersBody);

        string? userId = null;
        foreach (var user in membersDoc.RootElement.EnumerateArray())
        {
            if (user.GetProperty("email").GetString() == email)
            {
                userId = user.GetProperty("id").GetString();
                break;
            }
        }
        userId.Should().NotBeNullOrEmpty();

        // Deactivate
        var deactResp = await AdminClient.PostAsync($"/api/users/{userId}/deactivate", null);
        deactResp.StatusCode.Should().Be(HttpStatusCode.OK);

        // Reactivate
        var reactResp = await AdminClient.PostAsync($"/api/users/{userId}/reactivate", null);
        reactResp.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PromoteToLibrarian_AsRegularUser_Returns403()
    {
        var response = await Client.PostAsync("/api/users/some-id/promote-librarian", null);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
