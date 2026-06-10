using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using LMS.Tests.IntegrationTests;
using Xunit;

namespace LMS.Tests.IntegrationTests.Tests;

[Collection("Integration")]
public class AuthIntegrationTests : IClassFixture<LmsWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(LmsWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Should_Return_200_With_Valid_Data()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            firstName       = "Integration",
            lastName        = "Tester",
            email           = $"auth.test.{Guid.NewGuid()}@lms.com",
            password        = "Test@12345",
            confirmPassword = "Test@12345",
            phoneNumber     = "0000000000"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_Should_Return_400_With_Mismatched_Passwords()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            firstName       = "Integration",
            lastName        = "Tester",
            email           = $"mismatch.{Guid.NewGuid()}@lms.com",
            password        = "Test@12345",
            confirmPassword = "Wrong@12345",
            phoneNumber     = "0000000000"
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_Should_Return_Unauthorized_With_Wrong_Credentials()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email    = "nobody@lms.com",
            password = "WrongPass@1"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}