// DataSeeder.cs
using SchedulerApp.Data;
using SchedulerApp.Models.Entities;
using System;

public static class DataSeeder
{
    public static void SeedDevelopmentData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Users.Any())
        {
            var user1 = new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Email = "ante@mail.com",
                Name = "Ante",
                Surname = "Antic",
                Organization = "Prva firma",
                Phone = "123-456-7890"
            };

            var user2 = new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Email = "marin@mail.com",
                Name = "Marin",
                Surname = "Maric"
            };

            var room1 = new Room
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000101"),
                Address = "Ul. Rudera Boskovica 22",
                Capacity = 10,
                EmailAddress = "room1@example.com",
                FloorNumber = 1,
                DisplayName = "Conference Room A",
                IsWheelChairAccessible = true
            };

            var room2 = new Room
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000102"),
                Address = "456 Matica hrvatske",
                Capacity = 6,
                EmailAddress = "room2@example.com",
                FloorNumber = 2,
                DisplayName = "Meeting Room B",
                IsWheelChairAccessible = false
            };

            var booking1 = new Booking
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000001001"),
                Created_At = TimeOnly.Parse("09:00"),
                Date = DateOnly.Parse("2025-06-18"),
                Start_time = TimeOnly.Parse("10:00"),
                End_time = TimeOnly.Parse("11:00"),
                Description = "Prvi sastanak",
                User_id = user1.Id,
                Room_id = room1.Id
            };

            context.Users.AddRange(user1, user2);
            context.Rooms.AddRange(room1, room2);
            context.Bookings.Add(booking1);

            context.SaveChanges();
        }
    }
}
