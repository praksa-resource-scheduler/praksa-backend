namespace SchedulerApp.Models.Dtos
{
    public class BookingCreateDto
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Purpose { get; set; } = null!;
    }
}
