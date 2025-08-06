using ContosoPizza.Models;

namespace ContosoPizza.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<int> CreateWithItemsAsync(Order order);
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId);
    Task<IEnumerable<Order>> GetAllAsync();
}