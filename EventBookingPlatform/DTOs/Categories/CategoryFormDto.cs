using System.ComponentModel.DataAnnotations;

namespace EventBookingPlatform.DTOs.Categories;

public class CategoryFormDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(60)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Description { get; set; }
}
