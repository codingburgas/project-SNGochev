using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingPlatform.Models;

public class Event : BaseEntity
{
    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public DateTime EventDate { get; set; }

    [Required]
    [StringLength(120)]
    public string Venue { get; set; } = string.Empty;

    [Range(1, 100000)]
    public int Capacity { get; set; }

    [Range(typeof(decimal), "0.00", "1000000.00")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TicketPrice { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
