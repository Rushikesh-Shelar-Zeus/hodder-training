using ContosoPizza.Dtos.Pizza;

namespace ContosoPizza.Services.Interfaces;

public interface IPizzaService
{
    Task<IEnumerable<PizzaDto>> GetAllPizzasAsync();
    Task<PizzaDto?> GetPizzaByIdAsync(int id);
    Task<int> CreatePizzaAsync(CreatePizzaDto dto);
    Task<bool> UpdatePizzaAsync(int id, UpdatePizzaDto dto);
    Task<bool> DeletePizzaAsync(int id);
}