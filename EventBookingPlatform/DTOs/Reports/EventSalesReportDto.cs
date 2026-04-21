namespace EventBookingPlatform.DTOs.Reports;

public class EventSalesReportDto
{
    public string EventTitle { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int SoldSeats { get; set; }
    public decimal Revenue { get; set; }
    public decimal SoldPercentage { get; set; }
}
