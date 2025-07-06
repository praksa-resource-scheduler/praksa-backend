using SchedulerApp.Data;
using SchedulerApp.Models.Entities;
using System;

public static class DataSeeder
{
    public static void SeedDevelopmentData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();


        context.Bookings.RemoveRange(context.Bookings);
        context.Users.RemoveRange(context.Users);
        context.Rooms.RemoveRange(context.Rooms);
        context.SaveChanges();

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
                IsWheelchairAccessible = true
            };

            var room2 = new Room
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000102"),
                Address = "456 Matica hrvatske",
                Capacity = 6,
                EmailAddress = "room2@example.com",
                FloorNumber = 2,
                DisplayName = "Meeting Room B",
                IsWheelchairAccessible = false
            };

            var booking1 = new Booking
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000001001"),
                CreatedAt = TimeOnly.Parse("09:00"),
                Date = DateOnly.Parse("2025-06-18"),
                StartTime = TimeOnly.Parse("10:00"),
                EndTime = TimeOnly.Parse("11:00"),
                Purpose = "Prvi sastanak",
                UserId = user1.Id,
                RoomId = room1.Id
            };

            context.Users.AddRange(user1, user2);
            context.Rooms.AddRange(room1, room2);
            context.Bookings.Add(booking1);

            context.SaveChanges();
        }
    }
}
