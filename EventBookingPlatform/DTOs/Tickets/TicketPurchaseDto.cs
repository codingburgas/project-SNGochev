using System.ComponentModel.DataAnnotations;

namespace EventBookingPlatform.DTOs.Tickets;

public class TicketPurchaseDto
{
    [Required]
    public int EventId { get; set; }

    [Range(1, 20)]
    public int Quantity { get; set; } = 1;
}
