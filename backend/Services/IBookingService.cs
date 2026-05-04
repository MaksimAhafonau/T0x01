using SpaceDC.DTOs.Bookings;
using SpaceDC.Models.Enums;

namespace SpaceDC.Services;

public interface IBookingService
{
    Task<BookingDto> CreateAsync(Guid currentUserId, UserRole currentRole, CreateBookingRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookingDto>> ListAsync(Guid currentUserId, UserRole currentRole, Guid? userId, Guid? resourceId, CancellationToken cancellationToken = default);
    Task<BookingDto> UpdateStatusAsync(Guid currentUserId, UserRole currentRole, Guid bookingId, UpdateBookingStatusRequest request, CancellationToken cancellationToken = default);
}
