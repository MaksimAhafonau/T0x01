using SpaceDC.Models.Enums;

namespace SpaceDC.DTOs.Bookings;

public sealed class BookingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ResourceId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public BookingStatus Status { get; set; }
}
