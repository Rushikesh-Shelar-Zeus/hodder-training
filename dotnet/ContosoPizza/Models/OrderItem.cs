using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class OrderItem
{
    public int Id { get; set; }
    [Required]
    public int OrderId { get; set; }
    [Required]
    public int PizzaId { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal UnitPrice { get; set; }

    public Order? Order { get; set; }
    public Pizza? Pizza { get; set; }   
}