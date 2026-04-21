using EventBookingPlatform.DTOs.Reports;

namespace EventBookingPlatform.Services.Interfaces;

public interface IStatisticsService
{
    Task<ReportsDashboardDto> GetDashboardAsync(DateTime fromDate, DateTime toDate);
}
