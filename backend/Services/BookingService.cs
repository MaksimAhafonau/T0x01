using Microsoft.EntityFrameworkCore;
using SpaceDC.Data;
using SpaceDC.DTOs.Bookings;
using SpaceDC.Models;
using SpaceDC.Models.Enums;

namespace SpaceDC.Services;

public sealed class BookingService : IBookingService
{
    private readonly AppDbContext _db;

    public BookingService(AppDbContext db) => _db = db;

    public async Task<BookingDto> CreateAsync(Guid currentUserId, UserRole currentRole, CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        if (request.EndTime <= request.StartTime)
            throw new AppException(400, "End time must be after start time.");

        var resource = await _db.Resources
            .Include(r => r.Schedules)
            .FirstOrDefaultAsync(r => r.Id == request.ResourceId, cancellationToken);
        if (resource is null)
            throw new AppException(404, "Resource not found.");

        if (resource.Status != ResourceStatus.Available)
            throw new AppException(409, "Resource is not available for booking.");

        EnforceRoleForResourceType(currentRole, resource.Type);

        if (!FitsSchedule(resource, request.StartTime, request.EndTime))
            throw new AppException(409, "Booking is outside the resource schedule.");

        var overlap = await _db.Bookings.AnyAsync(
            b => b.ResourceId == resource.Id
                 && b.Status != BookingStatus.Cancelled
                 && b.StartTime < request.EndTime
                 && b.EndTime > request.StartTime,
            cancellationToken);
        if (overlap)
            throw new AppException(409, "The resource is already booked for this time range.");

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = currentUserId,
            ResourceId = resource.Id,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = BookingStatus.Confirmed
        };
        await _db.Bookings.AddAsync(booking, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return Map(booking);
    }

    public async Task<IReadOnlyList<BookingDto>> ListAsync(Guid currentUserId, UserRole currentRole, Guid? userId, Guid? resourceId, CancellationToken cancellationToken = default)
    {
        var isStaff = currentRole is UserRole.Teacher or UserRole.Admin;
        Guid? effectiveUserId = isStaff ? userId : currentUserId;
        if (!isStaff && userId.HasValue && userId.Value != currentUserId)
            throw new AppException(403, "You may only view your own bookings.");

        var q = _db.Bookings.AsNoTracking().AsQueryable();
        if (effectiveUserId.HasValue)
            q = q.Where(b => b.UserId == effectiveUserId.Value);
        if (resourceId.HasValue)
            q = q.Where(b => b.ResourceId == resourceId.Value);

        var list = await q.OrderBy(b => b.StartTime).ToListAsync(cancellationToken);
        return list.Select(Map).ToList();
    }

    public async Task<BookingDto> UpdateStatusAsync(Guid currentUserId, UserRole currentRole, Guid bookingId, UpdateBookingStatusRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
        if (booking is null)
            throw new AppException(404, "Booking not found.");

        var isStaff = currentRole is UserRole.Teacher or UserRole.Admin;
        if (!isStaff && booking.UserId != currentUserId)
            throw new AppException(403, "You may only update your own bookings.");

        booking.Status = request.Status;
        await _db.SaveChangesAsync(cancellationToken);
        return Map(booking);
    }

    private static void EnforceRoleForResourceType(UserRole role, ResourceType resourceType)
    {
        switch (role)
        {
            case UserRole.Student when resourceType != ResourceType.Table:
                throw new AppException(403, "Students may only book tables.");
            case UserRole.Teacher:
            case UserRole.Admin:
                break;
            case UserRole.Student:
                break;
            default:
                throw new AppException(403, "Role is not allowed to create bookings.");
        }
    }

    private static bool FitsSchedule(Resource resource, DateTimeOffset start, DateTimeOffset end)
    {
        if (resource.Schedules is null || resource.Schedules.Count == 0)
            return true;

        if (start.UtcDateTime.Date != end.UtcDateTime.Date)
            return false;

        var dow = start.UtcDateTime.DayOfWeek;
        var windows = resource.Schedules.Where(s => s.DayOfWeek == dow).ToList();
        if (windows.Count == 0)
            return false;

        var startT = TimeOnly.FromDateTime(start.UtcDateTime);
        var endT = TimeOnly.FromDateTime(end.UtcDateTime);
        return windows.Any(w => startT >= w.StartTime && endT <= w.EndTime);
    }

    private static BookingDto Map(Booking b) => new()
    {
        Id = b.Id,
        UserId = b.UserId,
        ResourceId = b.ResourceId,
        StartTime = b.StartTime,
        EndTime = b.EndTime,
        Status = b.Status
    };
}
