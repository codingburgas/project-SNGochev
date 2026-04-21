using EventBookingPlatform.Data;
using EventBookingPlatform.DTOs.Events;
using EventBookingPlatform.Models.Enums;
using EventBookingPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventBookingPlatform.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _db;

    public EventService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<EventListDto>> GetAllAsync(DateTime? fromDate, int? categoryId)
    {
        var query = _db.Events
            .AsNoTracking()
            .Include(e => e.Category)
            .Include(e => e.Tickets)
            .AsQueryable();

        if (fromDate.HasValue)
        {
            query = query.Where(e => e.EventDate.Date >= fromDate.Value.Date);
        }

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(e => e.CategoryId == categoryId.Value);
        }

        return await query
            .OrderBy(e => e.EventDate)
            .Select(e => new EventListDto
            {
                Id = e.Id,
                Title = e.Title,
                CategoryName = e.Category!.Name,
                EventDate = e.EventDate,
                Venue = e.Venue,
                Capacity = e.Capacity,
                SoldSeats = e.Tickets
                    .Where(t => t.Status == TicketStatus.Active || t.Status == TicketStatus.Used)
                    .Sum(t => t.Quantity),
                AvailableSeats = e.Capacity - e.Tickets
                    .Where(t => t.Status == TicketStatus.Active || t.Status == TicketStatus.Used)
                    .Sum(t => t.Quantity),
                TicketPrice = e.TicketPrice
            })
            .ToListAsync();
    }

    public async Task<EventDetailsDto?> GetByIdAsync(int id)
    {
        return await _db.Events
            .AsNoTracking()
            .Include(e => e.Category)
            .Include(e => e.Tickets)
            .Where(e => e.Id == id)
            .Select(e => new EventDetailsDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                CategoryId = e.CategoryId,
                CategoryName = e.Category!.Name,
                EventDate = e.EventDate,
                Venue = e.Venue,
                Capacity = e.Capacity,
                SoldSeats = e.Tickets
                    .Where(t => t.Status == TicketStatus.Active || t.Status == TicketStatus.Used)
                    .Sum(t => t.Quantity),
                AvailableSeats = e.Capacity - e.Tickets
                    .Where(t => t.Status == TicketStatus.Active || t.Status == TicketStatus.Used)
                    .Sum(t => t.Quantity),
                TicketPrice = e.TicketPrice,
                SoldPercentage = e.Capacity == 0
                    ? 0
                    : Math.Round((decimal)e.Tickets
                        .Where(t => t.Status == TicketStatus.Active || t.Status == TicketStatus.Used)
                        .Sum(t => t.Quantity) / e.Capacity * 100m, 2)
            })
            .FirstOrDefaultAsync();
    }

    public async Task<EventFormDto?> GetFormByIdAsync(int id)
    {
        return await _db.Events
            .Where(e => e.Id == id)
            .Select(e => new EventFormDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                EventDate = e.EventDate,
                Venue = e.Venue,
                Capacity = e.Capacity,
                TicketPrice = e.TicketPrice,
                CategoryId = e.CategoryId
            })
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(EventFormDto dto)
    {
        _db.Events.Add(new Models.Event
        {
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim(),
            EventDate = dto.EventDate,
            Venue = dto.Venue.Trim(),
            Capacity = dto.Capacity,
            TicketPrice = dto.TicketPrice,
            CategoryId = dto.CategoryId
        });

        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(EventFormDto dto)
    {
        var entity = await _db.Events.FindAsync(dto.Id);
        if (entity is null)
        {
            return;
        }

        entity.Title = dto.Title.Trim();
        entity.Description = dto.Description?.Trim();
        entity.EventDate = dto.EventDate;
        entity.Venue = dto.Venue.Trim();
        entity.Capacity = dto.Capacity;
        entity.TicketPrice = dto.TicketPrice;
        entity.CategoryId = dto.CategoryId;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.Events.FindAsync(id);
        if (entity is null)
        {
            return;
        }

        _db.Events.Remove(entity);
        await _db.SaveChangesAsync();
    }
}
