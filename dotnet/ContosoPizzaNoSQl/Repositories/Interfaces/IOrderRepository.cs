using ContosoPizzaNoSQl.Models;

namespace ContosoPizzaNoSQl.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAsync();
    Task<Order?> GetByIdAsync(string id);
    Task CreateAsync(Order order);
    Task UpdateAsync(string id, Order order);
    Task DeleteAsync(string id);
}