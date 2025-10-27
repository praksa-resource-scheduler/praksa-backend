using Microsoft.EntityFrameworkCore;
using SchedulerApp.Models.Entities;

namespace SchedulerApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Entities.Room> Rooms { get; set; } = null!;
        public DbSet<Models.Entities.User> Users { get; set; } = null!;
        public DbSet<Models.Entities.Booking> Bookings { get; set; } = null!;
        public DbSet<Models.Entities.ReservationRequest> ReservationRequests { get; set; } = null!;
        public DbSet<Models.Entities.Admin> Admins { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Models.Entities.Room>().ToTable("Rooms");
            modelBuilder.Entity<Models.Entities.User>().ToTable("Users");
            modelBuilder.Entity<Models.Entities.Booking>().ToTable("Bookings");
            modelBuilder.Entity<Models.Entities.ReservationRequest>().ToTable("ReservationRequests");
            modelBuilder.Entity<Models.Entities.Admin>().ToTable("Admins");
        }
    }
}
