using ContosoPizzaNoSQl.Models;

namespace ContosoPizzaNoSQl.Services.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetAllOrdersAsync();
    Task<List<Order>> GetOrdersAsync(int pageNumber, int pageSize, string sortBy, string order);
    Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId);
    Task<Order?> GetOrderByIdAsync(string id);
    Task<Order?> CreateOrderAsync(string customerId, List<OrderItem> orderItem);
    Task DeleteOrderAsync(string id);
}