namespace ContosoPizzaNoSQl.GraphQL.Orders;

public class OrderItemInput
{
    public string PizzaId { get; set; } = string.Empty;
    public int Quantity { get; set; }
}


