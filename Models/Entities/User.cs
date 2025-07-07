namespace SchedulerApp.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? Organization { get; set; }
        public string? Phone { get; set; }
        public int DailyLimitMinutes { get; set; } = 120;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
