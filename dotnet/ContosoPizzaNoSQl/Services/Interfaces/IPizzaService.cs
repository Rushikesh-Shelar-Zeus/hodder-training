using ContosoPizzaNoSQl.Models;

namespace ContosoPizzaNoSQl.Services.Interfaces;

public interface IPizzaService
{
    Task<List<Pizza>> GetPizzaAsync();
    Task<Pizza?> GetPizzaByIdAsync(string id);
    Task CreatePizzaAsync(Pizza pizza);
    Task<Pizza?> UpdatePizzaAsync(string id, Pizza pizza);
    Task DeletePizzaAsync(string id);
}