namespace EventBookingPlatform.DTOs.Reports;

public class CategoryStatisticsDto
{
    public string CategoryName { get; set; } = string.Empty;
    public int TotalSoldSeats { get; set; }
    public decimal Revenue { get; set; }
}
