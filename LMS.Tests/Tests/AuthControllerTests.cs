// ============================================================
//  FILE: Tests/AuthControllerTests.cs
//
//  Tests for:
//    POST /api/auth/register
//    POST /api/auth/login
//    POST /api/auth/logout
// ============================================================

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using LMS.Tests.Infrastructure;
using Xunit;

namespace LMS.Tests.Tests;

public class AuthControllerTests : TestFixture
{
    // ─────────────────────────────────────────────────────────────────────────
    // REGISTER
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Register_WithValidData_Returns200AndToken()
    {
        // Arrange – use a unique email so the test is isolated
        var email = $"register_valid_{Guid.NewGuid():N}@test.com";

        // Act
        var response = await CreateAnonymousClient().PostAsJsonAsync("/api/auth/register", new
        {
            firstName       = "Jane",
            lastName        = "Doe",
            middleName      = (string?)null,
            email,
            password        = "Test@12345",
            confirmPassword = "Test@12345",
            phoneNumber     = (string?)null
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);

        doc.RootElement.GetProperty("success").GetBoolean().Should().BeTrue();
        doc.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace(
            "a JWT token must be returned on successful registration");
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_Returns400()
    {
        // Arrange – register once, then try again with same email
        var email = $"dup_{Guid.NewGuid():N}@test.com";
        var payload = new
        {
            firstName       = "John",
            lastName        = "Smith",
            middleName      = (string?)null,
            email,
            password        = "Test@12345",
            confirmPassword = "Test@12345",
            phoneNumber     = (string?)null
        };

        var client = CreateAnonymousClient();
        await client.PostAsJsonAsync("/api/auth/register", payload); // first – succeeds

        // Act – second attempt with same email
        var response = await client.PostAsJsonAsync("/api/auth/register", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
            "registering the same email twice must be rejected");
    }

    [Fact]
    public async Task Register_WithWeakPassword_Returns400()
    {
        var response = await CreateAnonymousClient().PostAsJsonAsync("/api/auth/register", new
        {
            firstName       = "Weak",
            lastName        = "Pass",
            middleName      = (string?)null,
            email           = $"weak_{Guid.NewGuid():N}@test.com",
            password        = "123",     // too short / no complexity
            confirmPassword = "123",
            phoneNumber     = (string?)null
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest,
            "Identity should reject passwords that don't meet complexity rules");
    }

    [Fact]
    public async Task Register_WithMissingRequiredFields_Returns400()
    {
        // Missing lastName and email
        var response = await CreateAnonymousClient().PostAsJsonAsync("/api/auth/register", new
        {
            firstName = "NoLast",
            password  = "Test@12345"
        });

        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.UnprocessableEntity);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // LOGIN
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Login_WithValidCredentials_Returns200AndToken()
    {
        // Arrange – register fresh user
        var email    = $"login_ok_{Guid.NewGuid():N}@test.com";
        var password = "Test@12345";
        await EnsureUserRegistered(email, password, "Login", "Test");

        // Act
        var response = await CreateAnonymousClient().PostAsJsonAsync("/api/auth/login", new
        {
            email,
            password
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
        doc.RootElement.GetProperty("success").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task Login_WithWrongPassword_Returns401()
    {
        var response = await CreateAnonymousClient().PostAsJsonAsync("/api/auth/login", new
        {
            email    = TestUserEmail,
            password = "WrongPassword!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithNonExistentEmail_Returns401()
    {
        var response = await CreateAnonymousClient().PostAsJsonAsync("/api/auth/login", new
        {
            email    = "nobody@nowhere.com",
            password = "Test@12345"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // LOGOUT
    // ─────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Logout_WhenAuthenticated_Returns200()
    {
        var response = await Client.PostAsync("/api/auth/logout", null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Logout_WhenUnauthenticated_Returns401()
    {
        var response = await CreateAnonymousClient().PostAsync("/api/auth/logout", null);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized,
            "logout is protected with [Authorize]");
    }
}
