using ContosoPizza.Models;
using ContosoPizza.Dtos.Pizza;
using ContosoPizza.Services.Interfaces;
using ContosoPizza.Repositories.Interfaces;

namespace ContosoPizza.Services;

public class PizzaService : IPizzaService
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly ILogger<PizzaService> _logger;
    public PizzaService(IPizzaRepository pizzaRepository, ILogger<PizzaService> logger)
    {
        _pizzaRepository = pizzaRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<PizzaDto>> GetAllPizzasAsync()
    {
        try
        {
            var pizzas = await _pizzaRepository.GetAllAsync();
            return pizzas.Select(p => new PizzaDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                IsGlutenFree = p.IsGlutenFree
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all pizzas");
            return [];
        }
    }

    public async Task<PizzaDto?> GetPizzaByIdAsync(int id)
    {
        try
        {
            var pizza = await _pizzaRepository.GetByIdAsync(id);

            if (pizza is null) return null;

            return new PizzaDto
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Price = pizza.Price,
                IsGlutenFree = pizza.IsGlutenFree
            };
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Error retrieving pizza with ID {id}", id);
            return null;
        }
    }
    public async Task<int> CreatePizzaAsync(CreatePizzaDto dto)
    {
        try
        {
            var newPizza = new Pizza
            {
                Name = dto.Name,
                Price = dto.Price,
                IsGlutenFree = dto.IsGlutenFree
            };

            return await _pizzaRepository.CreateAsync(newPizza);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new pizza");
            return -1;
        }
    }

    public async Task<bool> UpdatePizzaAsync(int id, UpdatePizzaDto dto)
    {
        try
        {
            var pizza = new Pizza
            {
                Id = id,
                Name = dto.Name,
                Price = dto.Price,
                IsGlutenFree = dto.IsGlutenFree
            };

            return await _pizzaRepository.UpdateAsync(id, pizza);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating pizza with ID {id}", id);
            return false;
        }
    }

    public async Task<bool> DeletePizzaAsync(int id)
    {
        try
        {
            return await _pizzaRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting pizza with ID {id}", id);
            return false;
        }
    }
}