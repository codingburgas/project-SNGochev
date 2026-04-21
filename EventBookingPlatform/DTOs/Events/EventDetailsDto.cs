namespace EventBookingPlatform.DTOs.Events;

public class EventDetailsDto : EventListDto
{
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public decimal SoldPercentage { get; set; }
}
