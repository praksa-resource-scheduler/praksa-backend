using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerApp.Models.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }

        public required TimeOnly CreatedAt { get; set; }

        public required DateOnly Date { get; set; }

        public required TimeOnly StartTime { get; set; }

        public required TimeOnly EndTime { get; set; }

        public required string Purpose { get; set; }

        // Foreign key for User
        public required Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        // Foreign key for Room
        public required Guid RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = null!;
    }
}
