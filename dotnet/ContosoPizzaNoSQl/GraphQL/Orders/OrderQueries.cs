using ContosoPizzaNoSQl.GraphQL;
using ContosoPizzaNoSQl.GraphQL.SortTypes;
using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;

[ExtendObjectType(typeof(Query))]
public class OrderQueries
{
    public async Task<List<Order>> GetAllOrders([Service] IOrderService orderService)
    {
        return await orderService.GetAllOrdersAsync();
    }

    public async Task<Order?> GetOrderById(string id, [Service] IOrderService orderService)
    {
        return await orderService.GetOrderByIdAsync(id);
    }

    public async Task<List<Order>> GetOrders(OrderPagedInput input, [Service] IOrderService orderService)
    {
        return await orderService.GetOrdersAsync(pageNumber: input.PageNumber,
                                                  pageSize: input.PageSize,
                                                  sortBy: input.SortBy.ToString().ToLower(),
                                                  order: input.Order.ToString().ToLower());
    }
}