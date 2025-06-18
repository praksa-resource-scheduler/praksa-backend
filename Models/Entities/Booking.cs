using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerApp.Models.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }

        public required TimeOnly Created_At { get; set; }

        public required DateOnly Date { get; set; }

        public required TimeOnly Start_time { get; set; }

        public required TimeOnly End_time { get; set; }

        public required string Description { get; set; }

        // Foreign key for User
        public required Guid User_id { get; set; }
        [ForeignKey(nameof(User_id))]
        public User User { get; set; } = null!;

        // Foreign key for Room
        public required Guid Room_id { get; set; }
        [ForeignKey(nameof(Room_id))]
        public Room Room { get; set; } = null!;
    }
}
