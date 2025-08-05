using ContosoPizza.Models;

namespace ContosoPizza.Repositories.Interfaces;

public interface IPizzaRepository
{
    Task<IEnumerable<Pizza>> GetAllAsync();
    Task<Pizza?> GetByIdAsync(int id);
    Task<int> CreateAsync(Pizza pizza);
    Task<bool> UpdateAsync(int id, Pizza pizza);
    Task<bool> DeleteAsync(int id);
}