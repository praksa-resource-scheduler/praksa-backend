using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using SchedulerApp.Data;
using SchedulerApp.Models.Dtos;
using SchedulerApp.Models.Entities;
using SchedulerApp.Services;

namespace SchedulerApp.Controllers
{
    [ApiController]
    [Route("api/bookings")]
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

        [HttpPatch("{id}/modifyBooking")]
        public async Task<IActionResult> ModifyBooking(Guid id, [FromBody] BookingModifyDto dto)
        {
            var result = await _bookingService.ModifyBookingAsync(id, dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(new { message = result.Message });
        }
    }


}
