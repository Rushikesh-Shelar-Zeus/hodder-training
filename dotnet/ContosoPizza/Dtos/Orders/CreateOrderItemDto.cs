namespace ContosoPizza.Dtos.Orders;
public class CreateOrderItemDto
{
    public int PizzaId { get; set; }
    public int Quantity { get; set; }
}