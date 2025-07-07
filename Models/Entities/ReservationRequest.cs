using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerApp.Models.Entities
{
    public class ReservationRequest
    {
        public Guid Id { get; set; }

        public required DateTime CreatedAt { get; set; }
        public required DateTime StartTime { get; set; }
        public required DateTime EndTime { get; set; }
        public required string Purpose { get; set; }

        public required Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        public required Guid RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = null!;

        public string Status { get; set; } = "Pending";
    }

}
