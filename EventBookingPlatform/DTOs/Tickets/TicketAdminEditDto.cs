using System.ComponentModel.DataAnnotations;
using EventBookingPlatform.Models.Enums;

namespace EventBookingPlatform.DTOs.Tickets;

public class TicketAdminEditDto
{
    public int Id { get; set; }

    [Range(1, 20)]
    public int Quantity { get; set; }

    [Required]
    public TicketStatus Status { get; set; }
}
