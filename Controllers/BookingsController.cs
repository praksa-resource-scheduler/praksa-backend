using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulerApp.Data;
using SchedulerApp.Models.Entities;
using SchedulerApp.Models.Dtos;

namespace SchedulerApp.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
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
            if (dto.RoomId == Guid.Empty)
                return BadRequest("RoomId is required.");

            if (dto.UserId == Guid.Empty)
                return BadRequest("UserId is required.");

            if (string.IsNullOrWhiteSpace(dto.Purpose))
                return BadRequest("Purpose is required.");

            if (dto.StartTime >= dto.EndTime)
                return BadRequest("StartTime must be before EndTime.");

            var room = await _context.Rooms.FindAsync(dto.RoomId);
            if (room == null)
                return NotFound("Room not found.");

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return NotFound("User not found.");

            bool overlaps = await _context.Bookings.AnyAsync(b =>
                b.RoomId == dto.RoomId &&
                (
                    (dto.StartTime >= b.StartTime && dto.StartTime < b.EndTime) ||
                    (dto.EndTime > b.StartTime && dto.EndTime <= b.EndTime) ||
                    (dto.StartTime <= b.StartTime && dto.EndTime >= b.EndTime)
                ));
            if (overlaps)
                return Conflict("Reservation overlap.");


            var dailyLimit = TimeSpan.FromMinutes(user.DailyLimitMinutes);

            DateTime currentDate = dto.StartTime.Date;
            DateTime endDate = dto.EndTime.Date;

            while (currentDate <= endDate)
            {
                DateTime dayStart = currentDate;
                DateTime dayEnd = currentDate.AddDays(1);

                var userBookings = await _context.Bookings
                    .Where(b => b.UserId == dto.UserId &&
                                b.StartTime < dayEnd &&
                                b.EndTime > dayStart)
                    .ToListAsync();

                TimeSpan bookedThatDay = userBookings
                    .Aggregate(TimeSpan.Zero, (total, b) =>
                    {
                        var bookingStart = b.StartTime < dayStart ? dayStart : b.StartTime;
                        var bookingEnd = b.EndTime > dayEnd ? dayEnd : b.EndTime;
                        return total + (bookingEnd - bookingStart);
                    });

                var newStart = dto.StartTime < dayStart ? dayStart : dto.StartTime;
                var newEnd = dto.EndTime > dayEnd ? dayEnd : dto.EndTime;
                var newBookingDuration = newEnd > newStart ? newEnd - newStart : TimeSpan.Zero;

                var totalWithNew = bookedThatDay + newBookingDuration;

                if (totalWithNew > dailyLimit)
                {
                    return BadRequest(
                        $"Booking limit of {dailyLimit.TotalMinutes} minutes exceeded for {currentDate:yyyy-MM-dd}. " +
                        $"Already booked: {bookedThatDay.TotalMinutes} minutes. " +
                        $"Attempted to book additional {newBookingDuration.TotalMinutes} minutes, " +
                        $"exceeding the limit by {(bookedThatDay.TotalMinutes + newBookingDuration.TotalMinutes) - dailyLimit.TotalMinutes} minutes."
                    );
                }

                currentDate = currentDate.AddDays(1);
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
