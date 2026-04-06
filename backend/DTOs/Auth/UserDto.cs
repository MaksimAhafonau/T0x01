using SpaceDC.Models.Enums;

namespace SpaceDC.DTOs.Auth;

public sealed class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
