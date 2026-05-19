// ============================================================
//  FILE: Infrastructure/TestFixture.cs
//
//  A shared fixture that all test classes inherit from.
//  Provides:
//    - A pre-authenticated HttpClient (regular user token)
//    - An admin HttpClient (admin token)
//    - Helper methods: Login, Register, GetAuthToken
// ============================================================

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using LMS.Tests.Infrastructure;
using Xunit;
namespace LMS.Tests.Infrastructure;

/// <summary>
/// Base class for all integration tests.
/// Creates the factory once, gives each test a fresh HttpClient.
/// </summary>
public abstract class TestFixture : IAsyncLifetime
{
    protected readonly LmsApiFactory Factory;
    protected HttpClient Client = null!;      // regular authenticated user
    protected HttpClient AdminClient = null!; // admin authenticated user

    protected static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // Credentials used for the seeded test users
    protected const string TestUserEmail    = "testuser@lms.test";
    protected const string TestUserPassword = "Test@12345";
    protected const string AdminEmail       = "admin@lms.test";
    protected const string AdminPassword    = "Admin@12345";

    protected TestFixture()
    {
        Factory = new LmsApiFactory();
    }

    // ── Called once before each test class ───────────────────────────────────
    public virtual async Task InitializeAsync()
    {
        // Create plain clients first (unauthenticated)
        Client      = Factory.CreateClient();
        AdminClient = Factory.CreateClient();

        // Register & log in a regular user, attach JWT
        await EnsureUserRegistered(TestUserEmail, TestUserPassword, "Test", "User");
        var userToken = await GetAuthToken(TestUserEmail, TestUserPassword);
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", userToken);

        // Seed the admin user via the API's built-in seeding mechanism
        // (DatabaseInitializer creates admin@lms.test / Admin@12345 automatically)
        var adminToken = await GetAuthToken(AdminEmail, AdminPassword);
        AdminClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", adminToken);
    }

    // ── Called once after each test class ────────────────────────────────────
    public virtual Task DisposeAsync()
    {
        Client.Dispose();
        AdminClient.Dispose();
        Factory.Dispose();
        return Task.CompletedTask;
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Registers a user. Swallows errors if the user already exists.
    /// </summary>
    protected async Task EnsureUserRegistered(
        string email, string password, string firstName, string lastName)
    {
        var tempClient = Factory.CreateClient();
        await tempClient.PostAsJsonAsync("/api/auth/register", new
        {
            firstName,
            lastName,
            middleName  = (string?)null,
            email,
            password,
            confirmPassword = password,
            phoneNumber = (string?)null
        });
        // We ignore the response; if the user already exists the login below works anyway
    }

    /// <summary>
    /// Logs in and returns the raw JWT string. Throws if login fails.
    /// </summary>
    protected async Task<string> GetAuthToken(string email, string password)
    {
        var tempClient = Factory.CreateClient();
        var response = await tempClient.PostAsJsonAsync("/api/auth/login", new
        {
            email,
            password
        });

        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(
                $"Test setup failed – could not log in as {email}. " +
                $"Response: {(int)response.StatusCode} – {body}");

        using var doc = JsonDocument.Parse(body);
        var token = doc.RootElement.GetProperty("token").GetString();

        return token ?? throw new InvalidOperationException("Token was null in login response.");
    }

    /// <summary>
    /// Creates an unauthenticated client (for testing 401 scenarios).
    /// </summary>
    protected HttpClient CreateAnonymousClient() => Factory.CreateClient();
}
