using ContosoPizza.Models;

namespace ContosoPizza.Repositories;

public interface IPizzaRepository
{
    Task<IEnumerable<Pizza>> GetAllAsync();
    Task<Pizza?> GetByIdAsync(int id);
    Task<int> AddAsync(Pizza pizza);
    Task UpdateAsync(int id, Pizza pizza);
    Task DeleteAsync(int id);
}