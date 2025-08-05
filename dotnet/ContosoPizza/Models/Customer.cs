using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class Customer
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string? Email { get; set; }
    [Required, Phone]
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }

    public ICollection<Order>? Orders { get; set; } = [];
}