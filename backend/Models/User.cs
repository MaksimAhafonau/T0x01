using SpaceDC.Models.Enums;

namespace SpaceDC.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
