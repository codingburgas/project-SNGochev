using EventBookingPlatform.Data;
using EventBookingPlatform.DTOs.Tickets;
using EventBookingPlatform.Models;
using EventBookingPlatform.Models.Enums;
using EventBookingPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventBookingPlatform.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _db;

    public TicketService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TicketListDto>> GetAllAsync()
    {
        return await _db.Tickets
            .AsNoTracking()
            .Include(t => t.Event)
            .Include(t => t.ApplicationUser)
            .OrderByDescending(t => t.PurchasedAt)
            .Select(t => new TicketListDto
            {
                Id = t.Id,
                EventTitle = t.Event!.Title,
                ApplicationUserId = t.ApplicationUserId,
                BuyerName = t.ApplicationUser!.FullName,
                BuyerEmail = t.ApplicationUser!.Email,
                EventDate = t.Event!.EventDate,
                PurchasedAt = t.PurchasedAt,
                Quantity = t.Quantity,
                TotalPrice = t.TotalPrice,
                Status = t.Status
            })
            .ToListAsync();
    }

    public async Task<List<TicketListDto>> GetByUserAsync(int userId)
    {
        return await _db.Tickets
            .AsNoTracking()
            .Include(t => t.Event)
            .Include(t => t.ApplicationUser)
            .Where(t => t.ApplicationUserId == userId)
            .OrderByDescending(t => t.PurchasedAt)
            .Select(t => new TicketListDto
            {
                Id = t.Id,
                EventTitle = t.Event!.Title,
                ApplicationUserId = t.ApplicationUserId,
                BuyerName = t.ApplicationUser!.FullName,
                BuyerEmail = t.ApplicationUser!.Email,
                EventDate = t.Event!.EventDate,
                PurchasedAt = t.PurchasedAt,
                Quantity = t.Quantity,
                TotalPrice = t.TotalPrice,
                Status = t.Status
            })
            .ToListAsync();
    }

    public async Task<TicketListDto?> GetByIdAsync(int id)
    {
        return await _db.Tickets
            .AsNoTracking()
            .Include(t => t.Event)
            .Include(t => t.ApplicationUser)
            .Where(t => t.Id == id)
            .Select(t => new TicketListDto
            {
                Id = t.Id,
                EventTitle = t.Event!.Title,
                ApplicationUserId = t.ApplicationUserId,
                BuyerName = t.ApplicationUser!.FullName,
                BuyerEmail = t.ApplicationUser!.Email,
                EventDate = t.Event!.EventDate,
                PurchasedAt = t.PurchasedAt,
                Quantity = t.Quantity,
                TotalPrice = t.TotalPrice,
                Status = t.Status
            })
            .FirstOrDefaultAsync();
    }

    public async Task<(bool Succeeded, string Message)> PurchaseAsync(int userId, TicketPurchaseDto dto)
    {
        var eventEntity = await _db.Events
            .Include(e => e.Tickets)
            .FirstOrDefaultAsync(e => e.Id == dto.EventId);

        if (eventEntity is null)
        {
            return (false, "Event not found.");
        }

        if (eventEntity.EventDate <= DateTime.UtcNow)
        {
            return (false, "You cannot buy tickets for an event that has already started or ended.");
        }

        var soldSeats = eventEntity.Tickets
            .Where(t => t.Status == TicketStatus.Active || t.Status == TicketStatus.Used)
            .Sum(t => t.Quantity);

        var availableSeats = eventEntity.Capacity - soldSeats;

        if (dto.Quantity > availableSeats)
        {
            return (false, $"Only {availableSeats} seat(s) are available.");
        }

        var ticket = new Ticket
        {
            EventId = eventEntity.Id,
            ApplicationUserId = userId,
            Quantity = dto.Quantity,
            UnitPrice = eventEntity.TicketPrice,
            TotalPrice = eventEntity.TicketPrice * dto.Quantity,
            Status = TicketStatus.Active
        };

        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();

        return (true, "Ticket purchase completed successfully.");
    }

    public async Task<TicketAdminEditDto?> GetEditDtoAsync(int id)
    {
        return await _db.Tickets
            .Where(t => t.Id == id)
            .Select(t => new TicketAdminEditDto
            {
                Id = t.Id,
                Quantity = t.Quantity,
                Status = t.Status
            })
            .FirstOrDefaultAsync();
    }

    public async Task<(bool Succeeded, string Message)> UpdateAsync(TicketAdminEditDto dto)
    {
        var ticket = await _db.Tickets
            .Include(t => t.Event)
            .Include(t => t.Event!.Tickets)
            .FirstOrDefaultAsync(t => t.Id == dto.Id);

        if (ticket is null)
        {
            return (false, "Ticket not found.");
        }

        var activeStatuses = new[] { TicketStatus.Active, TicketStatus.Used };

        var soldSeatsWithoutCurrentTicket = ticket.Event!.Tickets
            .Where(t => t.Id != ticket.Id && activeStatuses.Contains(t.Status))
            .Sum(t => t.Quantity);

        var newQuantityCountsTowardCapacity = activeStatuses.Contains(dto.Status) ? dto.Quantity : 0;
        var wouldBeSoldSeats = soldSeatsWithoutCurrentTicket + newQuantityCountsTowardCapacity;

        if (wouldBeSoldSeats > ticket.Event.Capacity)
        {
            return (false, "The edited quantity exceeds the event capacity.");
        }

        ticket.Quantity = dto.Quantity;
        ticket.Status = dto.Status;
        ticket.TotalPrice = ticket.UnitPrice * dto.Quantity;

        await _db.SaveChangesAsync();

        return (true, "Ticket updated successfully.");
    }

    public async Task DeleteAsync(int id)
    {
        var ticket = await _db.Tickets.FindAsync(id);
        if (ticket is null)
        {
            return;
        }

        _db.Tickets.Remove(ticket);
        await _db.SaveChangesAsync();
    }
}
