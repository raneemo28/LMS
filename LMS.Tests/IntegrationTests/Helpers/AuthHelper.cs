using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LMS.Tests.IntegrationTests.Helpers;

public static class AuthHelper
{
    public static async Task<string> RegisterAndLoginAsync(
        HttpClient client,
        string email,
        string password = "Test@12345")
    {
        // Register — ignore failure if user already exists from a previous test run
        await client.PostAsJsonAsync("/api/auth/register", new
        {
            firstName = "Test",
            lastName  = "User",
            email,
            password,
            confirmPassword = password,
            phoneNumber = "0000000000"
        });

        var loginResp = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email,
            password
        });

        loginResp.EnsureSuccessStatusCode();
        var body = await loginResp.Content.ReadFromJsonAsync<LoginResponse>();
        return body!.Token;
    }

    private record LoginResponse(bool Success, string Token);
}