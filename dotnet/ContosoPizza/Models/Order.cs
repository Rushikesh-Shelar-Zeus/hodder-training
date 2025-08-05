using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class Order
{
    public int Id { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    [Required]
    public decimal TotalAmount { get; set; }

    public Customer? Customer { get; set; }
    public ICollection<OrderItem>? OrderItems { get; set; } = [];
}