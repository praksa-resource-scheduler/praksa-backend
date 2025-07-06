namespace SchedulerApp.Models.Dtos
{
    public class BookingCreateDto
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Purpose { get; set; } = null!;
    }
}
