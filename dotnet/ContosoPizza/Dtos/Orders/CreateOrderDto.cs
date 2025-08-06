using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Dtos.Orders;

public class CreateOrderDto
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public List<CreateOrderItemDto> OrderItems { get; set; } = [];
}