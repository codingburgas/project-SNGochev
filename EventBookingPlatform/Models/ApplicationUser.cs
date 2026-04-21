using System.ComponentModel.DataAnnotations;

namespace EventBookingPlatform.Models;

public class ApplicationUser : BaseEntity
{
    [Required]
    [StringLength(60)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(60)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Role { get; set; } = "Attendee";

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public string FullName => $"{FirstName} {LastName}";
}
