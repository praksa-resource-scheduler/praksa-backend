using Microsoft.EntityFrameworkCore;

namespace SchedulerApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Entities.Room> Rooms { get; set; } = null!;
        public DbSet<Models.Entities.User> Users { get; set; } = null!;
        public DbSet<Models.Entities.Booking> Bookings { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Models.Entities.Room>().ToTable("Rooms");
            modelBuilder.Entity<Models.Entities.User>().ToTable("Users");
            modelBuilder.Entity<Models.Entities.Booking>().ToTable("Bookings");
        }
    }
}
