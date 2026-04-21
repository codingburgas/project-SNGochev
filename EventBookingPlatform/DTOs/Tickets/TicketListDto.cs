using EventBookingPlatform.Models.Enums;

namespace EventBookingPlatform.DTOs.Tickets;

public class TicketListDto
{
    public int Id { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public int ApplicationUserId { get; set; }
    public string BuyerName { get; set; } = string.Empty;
    public string BuyerEmail { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public DateTime PurchasedAt { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public TicketStatus Status { get; set; }
}
