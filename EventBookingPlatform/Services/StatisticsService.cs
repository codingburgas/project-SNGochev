using EventBookingPlatform.Data;
using EventBookingPlatform.DTOs.Reports;
using EventBookingPlatform.Models.Enums;
using EventBookingPlatform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventBookingPlatform.Services;

public class StatisticsService : IStatisticsService
{
    private readonly AppDbContext _db;

    public StatisticsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ReportsDashboardDto> GetDashboardAsync(DateTime fromDate, DateTime toDate)
    {
        var validStatuses = new[] { TicketStatus.Active, TicketStatus.Used };

        var ticketsQuery = _db.Tickets
            .AsNoTracking()
            .Include(t => t.Event)
            .ThenInclude(e => e!.Category)
            .Where(t => t.PurchasedAt.Date >= fromDate.Date && t.PurchasedAt.Date <= toDate.Date)
            .Where(t => validStatuses.Contains(t.Status));

        var eventStats = await ticketsQuery
            .GroupBy(t => new
            {
                t.EventId,
                t.Event!.Title,
                CategoryName = t.Event.Category!.Name,
                t.Event.Capacity
            })
            .Select(g => new EventSalesReportDto
            {
                EventTitle = g.Key.Title,
                CategoryName = g.Key.CategoryName,
                Capacity = g.Key.Capacity,
                SoldSeats = g.Sum(x => x.Quantity),
                Revenue = g.Sum(x => x.TotalPrice),
                SoldPercentage = g.Key.Capacity == 0 ? 0 : Math.Round((decimal)g.Sum(x => x.Quantity) / g.Key.Capacity * 100m, 2)
            })
            .OrderByDescending(x => x.Revenue)
            .ToListAsync();

        var categoryStats = await ticketsQuery
            .GroupBy(t => t.Event!.Category!.Name)
            .Select(g => new CategoryStatisticsDto
            {
                CategoryName = g.Key,
                TotalSoldSeats = g.Sum(x => x.Quantity),
                Revenue = g.Sum(x => x.TotalPrice)
            })
            .OrderByDescending(x => x.Revenue)
            .ToListAsync();

        return new ReportsDashboardDto
        {
            FromDate = fromDate,
            ToDate = toDate,
            TotalRevenue = await ticketsQuery.SumAsync(t => (decimal?)t.TotalPrice) ?? 0m,
            Events = eventStats,
            Categories = categoryStats
        };
    }
}
