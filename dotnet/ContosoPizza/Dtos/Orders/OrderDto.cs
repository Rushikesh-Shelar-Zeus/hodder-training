namespace ContosoPizza.Dtos.Orders;

public class OrderDto
{
    public int Id { get; set; }
    public string? CustomerName { get;  set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    
    public List<OrderItemDto> OrderItems { get; set; } = [];
}