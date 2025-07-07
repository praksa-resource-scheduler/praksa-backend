using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulerApp.Data;
using SchedulerApp.Models.Dtos;
using SchedulerApp.Models.Entities;
using SchedulerApp.Services;

namespace SchedulerApp.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly BookingService _bookingService;

        public BookingsController(AppDbContext context, BookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking([FromBody] BookingCreateDto dto)
        {
            var validationError = await _bookingService.ValidateBookingAsync(dto);
            if (validationError != null)
            {
                if (validationError == "Reservation overlap.")
                    return Conflict(validationError);
                return BadRequest(validationError);
            }
         
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                StartTime = DateTime.SpecifyKind(dto.StartTime, DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(dto.EndTime, DateTimeKind.Utc),
                Purpose = dto.Purpose,
                RoomId = dto.RoomId,
                UserId = dto.UserId
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookings), new { id = booking.Id }, booking);
        }
    }

}
