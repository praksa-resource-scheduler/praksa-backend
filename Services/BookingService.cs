using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SchedulerApp.Data;
using SchedulerApp.Models.Dtos;

namespace SchedulerApp.Services
{
    public class BookingService
    {
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> ValidateBookingAsync(BookingCreateDto dto)
        {
            if (dto.RoomId == Guid.Empty)
                return "RoomId is required.";

            if (dto.UserId == Guid.Empty)
                return "UserId is required.";

            if (string.IsNullOrWhiteSpace(dto.Purpose))
                return "Purpose is required.";

            if (dto.StartTime >= dto.EndTime)
                return "StartTime must be before EndTime.";

            var room = await _context.Rooms.FindAsync(dto.RoomId);
            if (room == null)
                return "Room not found.";

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return "User not found.";

            bool overlaps = await _context.Bookings.AnyAsync(b =>
                b.RoomId == dto.RoomId &&
                (
                    (dto.StartTime >= b.StartTime && dto.StartTime < b.EndTime) ||
                    (dto.EndTime > b.StartTime && dto.EndTime <= b.EndTime) ||
                    (dto.StartTime <= b.StartTime && dto.EndTime >= b.EndTime)
                ));
            if (overlaps)
                return "Reservation overlap.";

            string? limitMessage = await CheckUserDailyLimitAsync(dto, user.DailyLimitMinutes);
            if (limitMessage != null)
                return limitMessage;

            return null;
        }

        private async Task<string?> CheckUserDailyLimitAsync(BookingCreateDto dto, int dailyLimitMinutes)
        {
            var dailyLimit = TimeSpan.FromMinutes(dailyLimitMinutes);
            var currentDate = dto.StartTime.Date;
            var endDate = dto.EndTime.Date;

            while (currentDate <= endDate)
            {
                DateTime dayStart = currentDate;
                DateTime dayEnd = currentDate.AddDays(1);

                var userBookings = await _context.Bookings
                    .Where(b => b.UserId == dto.UserId &&
                                b.StartTime < dayEnd &&
                                b.EndTime > dayStart)
                    .ToListAsync();

                TimeSpan bookedThatDay = userBookings.Aggregate(TimeSpan.Zero, (total, b) =>
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
                    return
                        $"Booking limit of {dailyLimit.TotalMinutes} minutes exceeded for {currentDate:yyyy-MM-dd}. " +
                        $"Already booked: {bookedThatDay.TotalMinutes} minutes. " +
                        $"Attempted to book additional {newBookingDuration.TotalMinutes} minutes, " +
                        $"exceeding the limit by {(totalWithNew - dailyLimit).TotalMinutes} minutes.";
                }

                currentDate = currentDate.AddDays(1);
            }

            return null;
        }
    }
}
