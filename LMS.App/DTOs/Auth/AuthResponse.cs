namespace LMS.App.DTOs.Auth;

public record AuthResponse
{
    public string Token    { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email    { get; init; } = string.Empty;
    public string Role     { get; init; } = string.Empty;
    public bool   Success  { get; init; }
    public string Message  { get; init; } = string.Empty;
}