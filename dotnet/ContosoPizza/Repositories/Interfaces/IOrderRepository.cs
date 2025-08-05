using ContosoPizza.Models;

namespace ContosoPizza.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string sortDirection = "asc");
    Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId);
    Task<Order?> GetByIdAsync(int id);
    Task<int> CreateWithItemsAsync(Order order);
    Task<bool> DeleteWithItemsAsync(int id);
}