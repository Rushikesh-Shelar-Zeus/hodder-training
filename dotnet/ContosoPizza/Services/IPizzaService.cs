using System;
using ContosoPizza.Models;

namespace ContosoPizza.Services;

public interface IPizzaService
{
    Task<IEnumerable<Pizza>> GetAllPizzasAsync();
    Task<Pizza?> GetPizzaByIdAsync(int id);
    Task<int?> AddPizzaAsync(Pizza pizza);
    Task UpdatePizzaAsync(int id, Pizza pizza);
    Task DeletePizzaAsync(int id);
}
