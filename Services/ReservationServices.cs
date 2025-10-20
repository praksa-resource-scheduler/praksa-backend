using Microsoft.EntityFrameworkCore;
using SchedulerApp.Data;
using SchedulerApp.Models.Dtos;
using SchedulerApp.Models.Entities;

namespace SchedulerApp.Services
{
    public class ReservationServices
    {
        private readonly AppDbContext _context;
        private readonly BookingService _bookingService;

        public ReservationServices(AppDbContext context, BookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        public async Task<(bool IsSuccess, string Message)> AcceptReservationRequestAsync(Guid requestId)
        {
            var request = await _context.ReservationRequests.FindAsync(requestId);
            if (request == null)
                return (false, "Reservation request not found.");

            if (request.Status != "Pending")
                return (false, "Reservation request is not pending, can only accept pending requests.");

            var bookingDto = new BookingCreateDto
            {
                RoomId = request.RoomId,
                UserId = request.UserId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Purpose = request.Purpose
            };
            var validationError = await _bookingService.ValidateBookingAsync(bookingDto);

            if (!string.IsNullOrEmpty(validationError))
                return (false, validationError);

            // Use a transaction for atomicity
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    StartTime = DateTime.SpecifyKind(request.StartTime, DateTimeKind.Utc),
                    EndTime = DateTime.SpecifyKind(request.EndTime, DateTimeKind.Utc),
                    Purpose = request.Purpose,
                    RoomId = request.RoomId,
                    UserId = request.UserId
                };

                _context.Bookings.Add(booking);

                request.Status = "Accepted";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Reservation request accepted and booking created.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Error accepting reservation request: {ex.Message}");
            }
        }
    }
}