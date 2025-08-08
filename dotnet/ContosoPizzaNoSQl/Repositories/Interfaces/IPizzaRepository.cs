
using ContosoPizzaNoSQl.Models;

namespace ContosoPizzaNoSQl.Repositories.Interfaces;

public interface IPizzaRepository
{
    Task<List<Pizza>> GetAsync();
    Task<Pizza?> GetByIdAsync(string id);
    Task CreateAsync(Pizza pizza);
    Task UpdateAsync(string id, Pizza pizza);
    Task DeleteAsync(string id);
}