using SpaceDC.Models.Enums;

namespace SpaceDC.DTOs.Bookings;

public sealed class UpdateBookingStatusRequest
{
    public BookingStatus Status { get; set; }
}
