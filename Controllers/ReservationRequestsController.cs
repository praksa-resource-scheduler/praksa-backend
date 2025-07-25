using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulerApp.Data;
using SchedulerApp.Models.Dtos;
using SchedulerApp.Models.Entities;
using SchedulerApp.Services;

namespace SchedulerApp.Controllers
{
    [ApiController]
    [Route("api/reservation-requests")]
    public class ReservationRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly BookingService _bookingService;

        public ReservationRequestsController(AppDbContext context, BookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationRequest>>> GetReservationRequests()
        {
            return await _context.ReservationRequests
                .Include(r => r.User)
                .Include(r => r.Room)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ReservationRequest>> CreateReservationRequest([FromBody] BookingCreateDto dto)
        {
            var validationError = await _bookingService.ValidateBookingAsync(dto);
            if (validationError != null)
            {
                if (validationError == "Reservation overlap.")
                    return Conflict(validationError);
                return BadRequest(validationError);
            }

            var reservationRequest = new ReservationRequest
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                StartTime = DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(dto.EndTime, DateTimeKind.Utc),
                Purpose = dto.Purpose,
                RoomId = dto.RoomId,
                UserId = dto.UserId,
                Status = "Pending"
            };

            _context.ReservationRequests.Add(reservationRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservationRequests), new { id = reservationRequest.Id }, reservationRequest);
        }
    }
}
