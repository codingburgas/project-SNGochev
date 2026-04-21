using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventBookingPlatform.Models.Enums;

namespace EventBookingPlatform.Models;

public class Ticket : BaseEntity
{
    [Range(1, 20)]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    public TicketStatus Status { get; set; } = TicketStatus.Active;

    public int EventId { get; set; }
    public Event? Event { get; set; }

    public int ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
}
