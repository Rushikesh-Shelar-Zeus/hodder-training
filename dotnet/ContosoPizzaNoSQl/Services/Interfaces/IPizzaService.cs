using ContosoPizzaNoSQl.Models;

namespace ContosoPizzaNoSQl.Services.Interfaces;

public interface IPizzaService
{
    Task<PagedOutput<Pizza>> GetPizzaAsync(int pageNumber, int pageSize, string sortBy, string order);
    Task<Pizza?> GetPizzaByIdAsync(string id);
    Task CreatePizzaAsync(Pizza pizza);
    Task<Pizza?> UpdatePizzaAsync(string id, Pizza pizza);
    Task DeletePizzaAsync(string id);
    Task<List<Pizza>> GetAllPizzaAsync();

}