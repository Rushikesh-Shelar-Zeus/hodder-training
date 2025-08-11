using ContosoPizzaNoSQl.Models;
using MongoDB.Driver;

namespace ContosoPizzaNoSQl.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(string id);
    Task<List<Order>> GetAsync(int pageNumber, int pageSize, SortDefinition<Order> sortDefinition);
    Task<List<Order>> GetByCustomerIdAsync(string customerId);
    Task CreateAsync(Order order);
    Task UpdateAsync(string id, Order order);
    Task DeleteAsync(string id);
}