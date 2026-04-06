namespace SpaceDC.DTOs.Bookings;

public sealed class CreateBookingRequest
{
    public Guid ResourceId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
}
