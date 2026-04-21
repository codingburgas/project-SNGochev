# Hand-in Checklist

## Included in this package
- Full C# ASP.NET Core MVC source code
- EF Core DbContext and migrations
- Models, DTOs, interfaces, services and controllers
- Razor Views with Bootstrap UI
- Authentication and authorization by roles
- Ticket purchase logic and reports
- README documentation
- Wiki starter page

## What to mention to the teacher
- The code is written in **English** as required
- The project uses **Code-First EF Core**
- The database is **SQLite** and persists after restart
- It demonstrates:
  - OOP
  - MVC architecture
  - Dependency Injection
  - DTOs
  - LINQ
  - CRUD
  - Validation
  - Roles and authorization
  - Reports/statistics

## Important note
This environment did not have the .NET SDK installed, so the project files were prepared directly without running a live build here.
On a computer with the .NET 8 SDK installed, run:

```bash
dotnet restore
dotnet run
```

## Demo users
- Admin: admin@eventbooking.local / Admin123!
- Organizer: organizer@eventbooking.local / Organizer123!
- Attendee: user@eventbooking.local / User123!
