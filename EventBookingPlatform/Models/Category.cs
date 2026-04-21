using System.ComponentModel.DataAnnotations;

namespace EventBookingPlatform.Models;

public class Category : BaseEntity
{
    [Required]
    [StringLength(60)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }

    public ICollection<Event> Events { get; set; } = new List<Event>();
}
