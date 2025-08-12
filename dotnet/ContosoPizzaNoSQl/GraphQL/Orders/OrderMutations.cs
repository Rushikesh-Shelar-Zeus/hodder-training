using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;

namespace ContosoPizzaNoSQl.GraphQL.Orders;

[ExtendObjectType(typeof(Mutation))]
public class OrderMutations
{
    public async Task<Order?> CreateOrder(CreateOrderInput input, [Service] IOrderService orderService)
    {
        var order = new Order
        {
            CustomerId = input.CustomerId,
            OrderItems = [.. input.OrderItems.Select(item => new OrderItem
            {
                PizzaId = item.PizzaId,
                Quantity = item.Quantity
            })]
        };
        Console.WriteLine($"Creating order for customer {input.CustomerId} with {input.OrderItems.Count} items.");
        var createdOrder = await orderService.CreateOrderAsync(input.CustomerId, order.OrderItems);
        Console.WriteLine($"Order created with ID: {createdOrder?.Id}");
        return createdOrder;
    }

    public async Task<bool> DeleteOrder(string id, [Service] IOrderService orderService)
    {
        await orderService.DeleteOrderAsync(id);
        return true;
    }
}