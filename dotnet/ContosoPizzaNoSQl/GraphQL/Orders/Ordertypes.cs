
namespace ContosoPizzaNoSQl.GraphQL.Orders;

public class CreateOrderInput
{
    public string CustomerId { get; set; } = string.Empty;
    public List<OrderItemInput> OrderItems { get; set; } = [];
}