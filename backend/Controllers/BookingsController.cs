using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceDC.DTOs.Bookings;
using SpaceDC.Services;

namespace SpaceDC.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public sealed class BookingsController : ControllerBase
{
    private readonly IBookingService _bookings;

    public BookingsController(IBookingService bookings) => _bookings = bookings;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookingDto>>> List(
        [FromQuery] Guid? userId,
        [FromQuery] Guid? resourceId,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.GetUserId();
        var role = User.GetUserRole();
        var list = await _bookings.ListAsync(userIdClaim, role, userId, resourceId, cancellationToken);
        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var role = User.GetUserRole();
        var dto = await _bookings.CreateAsync(userId, role, request, cancellationToken);
        return Created($"{Request.Path}/{dto.Id}", dto);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<BookingDto>> UpdateStatus(
        Guid id,
        [FromBody] UpdateBookingStatusRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var role = User.GetUserRole();
        var dto = await _bookings.UpdateStatusAsync(userId, role, id, request, cancellationToken);
        return Ok(dto);
    }
}
