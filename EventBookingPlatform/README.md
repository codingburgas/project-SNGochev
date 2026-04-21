# Event Booking Platform

ASP.NET Core MVC project for **Project Activity – Grade 11 (2025/2026)**.

This project follows the assignment requirements for:
- ASP.NET Core MVC architecture
- EF Core Code-First + migrations
- LINQ queries and related entities
- OOP principles (BaseEntity, interfaces, services, encapsulation)
- DTO usage
- Authentication and authorization with roles
- CRUD for Categories, Events and Tickets
- Ticket purchase logic with capacity tracking
- Reports and statistics
- Bootstrap UI and client-side validation

## Technologies
- C#
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQLite
- Bootstrap 5
- Cookie Authentication

## Roles
- **Admin** – full access
- **Organizer** – create/edit events and categories, view reports, manage tickets
- **Attendee** – browse events and purchase tickets

## Demo accounts
- **Admin**: `admin@eventbooking.local` / `Admin123!`
- **Organizer**: `organizer@eventbooking.local` / `Organizer123!`
- **Attendee**: `user@eventbooking.local` / `User123!`

## Main features
### Categories
- Create, read, update and delete categories
- Category details and event count

### Events
- Create, read, update and delete events
- Filter by date and category
- Show capacity, sold seats and available seats

### Ticket system
- Logged-in users can buy tickets
- The app checks event capacity before purchase
- Admin/Organizer can view all tickets
- Admin/Organizer can edit ticket quantity and status
- Admin can delete tickets

### Reports
- Sold percentage for each event
- Total revenue for a selected period
- Revenue and sold seats by category

## Database design
Tables:
- `Categories`
- `Events`
- `Users`
- `Tickets`

Relationships:
- `Category -> Event` = one-to-many
- `Event -> Ticket` = one-to-many
- `User -> Ticket` = one-to-many
- `User <-> Event` = many-to-many through `Ticket`

## LINQ used in the project
Examples:
- Filter events by date
- Search events by category
- Calculate sold seats and remaining seats
- Calculate event sold percentage
- Revenue totals for selected period
- Category statistics

## How to run
1. Install **.NET 8 SDK**
2. Open the project folder in terminal
3. Restore packages:
   ```bash
   dotnet restore
   ```
4. Run the project:
   ```bash
   dotnet run
   ```
5. Open the local URL shown in the terminal

## Notes
- The application uses **SQLite** with a local file: `eventbooking.db`
- Data persists between restarts
- On first run, the database is migrated and seeded automatically

## Suggested Git commits
- Initial ASP.NET Core MVC project setup
- Add data models and DbContext
- Add migrations and SQLite configuration
- Add services, interfaces and DTOs
- Implement event and category CRUD
- Implement ticket purchase logic
- Add authentication and role authorization
- Add reports and Bootstrap styling
- Add README and wiki documentation
