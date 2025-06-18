namespace SchedulerApp.Models.Entities
{
    public class Room
    {
        public Guid Id { get; set; }

        public required string Address { get; set; }
        public required int Capacity { get; set; }
        public required string EmailAddress { get; set; }
        public required int FloorNumber { get; set; }
        public required string DisplayName { get; set; }
        public required bool IsWheelChairAccessible { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
