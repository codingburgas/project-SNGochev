using EventBookingPlatform.Models;
using Microsoft.AspNetCore.Identity;

namespace EventBookingPlatform.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!db.Categories.Any())
        {
            db.Categories.AddRange(
                new Category { Name = "Music", Description = "Concerts, festivals and live performances" },
                new Category { Name = "Sports", Description = "Football, basketball, marathons and more" },
                new Category { Name = "Conference", Description = "Business, education and tech conferences" });
        }

        var hasher = new PasswordHasher<ApplicationUser>();

        if (!db.Users.Any())
        {
            var admin = new ApplicationUser
            {
                FirstName = "System",
                LastName = "Admin",
                Email = "admin@eventbooking.local",
                Role = "Admin"
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");

            var organizer = new ApplicationUser
            {
                FirstName = "Main",
                LastName = "Organizer",
                Email = "organizer@eventbooking.local",
                Role = "Organizer"
            };
            organizer.PasswordHash = hasher.HashPassword(organizer, "Organizer123!");

            var attendee = new ApplicationUser
            {
                FirstName = "Demo",
                LastName = "User",
                Email = "user@eventbooking.local",
                Role = "Attendee"
            };
            attendee.PasswordHash = hasher.HashPassword(attendee, "User123!")
;
            db.Users.AddRange(admin, organizer, attendee);
            await db.SaveChangesAsync();
        }

        if (!db.Events.Any())
        {
            var musicCategory = db.Categories.First(c => c.Name == "Music");
            var sportsCategory = db.Categories.First(c => c.Name == "Sports");
            var conferenceCategory = db.Categories.First(c => c.Name == "Conference");

            db.Events.AddRange(
                new Event
                {
                    Title = "Burgas Summer Concert",
                    Description = "Open-air music event with local and international artists.",
                    EventDate = DateTime.UtcNow.Date.AddDays(10).AddHours(19),
                    Venue = "Sea Garden Stage",
                    Capacity = 500,
                    TicketPrice = 25.00m,
                    CategoryId = musicCategory.Id
                },
                new Event
                {
                    Title = "Regional Football Cup",
                    Description = "Weekend football tournament with youth teams.",
                    EventDate = DateTime.UtcNow.Date.AddDays(15).AddHours(16),
                    Venue = "City Stadium",
                    Capacity = 1200,
                    TicketPrice = 18.00m,
                    CategoryId = sportsCategory.Id
                },
                new Event
                {
                    Title = "Tech Innovators Forum",
                    Description = "Conference about software, startups and AI.",
                    EventDate = DateTime.UtcNow.Date.AddDays(30).AddHours(9),
                    Venue = "Convention Center",
                    Capacity = 300,
                    TicketPrice = 60.00m,
                    CategoryId = conferenceCategory.Id
                });
        }

        await db.SaveChangesAsync();
    }
}
