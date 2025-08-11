using ContosoPizzaNoSQl.GraphQL;
using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;

[ExtendObjectType(typeof(Query))]
public class OrderQueries
{
    public async Task<List<Order>> GetOrders([Service] IOrderService orderService)
    {
        return await orderService.GetOrdersAsync();
    }

    public async Task<Order?> GetOrderById(string id, [Service] IOrderService orderService)
    {
        return await orderService.GetOrderByIdAsync(id);
    }
}