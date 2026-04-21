namespace EventBookingPlatform.DTOs.Reports;

public class ReportsDashboardDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<EventSalesReportDto> Events { get; set; } = new();
    public List<CategoryStatisticsDto> Categories { get; set; } = new();
}
