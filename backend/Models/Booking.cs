using SpaceDC.Models.Enums;

namespace SpaceDC.Models;

public class Booking
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ResourceId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public BookingStatus Status { get; set; }

    public User User { get; set; } = null!;
    public Resource Resource { get; set; } = null!;
}
