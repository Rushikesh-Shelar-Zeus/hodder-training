
using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Dtos.Customers;

public class CreateCustomerDto
{
    [Required]
    public string? Name { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string? Email { get; set; } = string.Empty;
    [Required, Phone]
    public string? PhoneNumber { get; set; } = string.Empty;
    [Required]
    public string? Address { get; set; } = string.Empty;
}