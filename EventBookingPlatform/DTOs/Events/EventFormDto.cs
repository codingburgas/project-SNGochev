using System.ComponentModel.DataAnnotations;

namespace EventBookingPlatform.DTOs.Events;

public class EventFormDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Date and Time")]
    public DateTime EventDate { get; set; } = DateTime.UtcNow.AddDays(1);

    [Required]
    [StringLength(120)]
    public string Venue { get; set; } = string.Empty;

    [Range(1, 100000)]
    public int Capacity { get; set; }

    [Range(typeof(decimal), "0.00", "1000000.00")]
    [Display(Name = "Ticket Price")]
    public decimal TicketPrice { get; set; }

    [Display(Name = "Category")]
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}
