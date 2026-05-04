namespace SpaceDC.DTOs.Auth;

public sealed class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
}
