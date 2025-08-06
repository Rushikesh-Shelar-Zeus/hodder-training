namespace ContosoPizza.Dtos.Orders;

public class OrderItemDto
{
    public int PizzaId { get; set; }
    public string? PizzaName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
}