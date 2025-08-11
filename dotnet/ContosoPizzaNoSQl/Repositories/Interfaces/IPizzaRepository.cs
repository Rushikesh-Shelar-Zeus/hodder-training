
using ContosoPizzaNoSQl.Models;
using MongoDB.Driver;

namespace ContosoPizzaNoSQl.Repositories.Interfaces;

public interface IPizzaRepository
{
    Task<List<Pizza>> GetAsync(int pageNumber, int pageSize, SortDefinition<Pizza> sortDefinition);
    Task<List<Pizza>> GetAllAsync();
    Task<Pizza?> GetByIdAsync(string id);
    Task CreateAsync(Pizza pizza);
    Task UpdateAsync(string id, Pizza pizza);
    Task DeleteAsync(string id);
    Task<int> CountAsync();
}