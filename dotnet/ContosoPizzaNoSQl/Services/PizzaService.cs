using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;
using ContosoPizzaNoSQl.Repositories.Interfaces;

namespace ContosoPizzaNoSQl.Services;

public class PizzaService : IPizzaService
{
    private readonly IPizzaRepository _pizzaRepository;

    public PizzaService(IPizzaRepository pizzaRepository)
    {
        _pizzaRepository = pizzaRepository;
    }

    public async Task CreatePizzaAsync(Pizza pizza)
    {
        await _pizzaRepository.CreateAsync(pizza);
    }

    public async Task DeletePizzaAsync(string id)
    {
        await _pizzaRepository.DeleteAsync(id);
    }

    public async Task<Pizza?> GetPizzaByIdAsync(string id)
    {
       return await _pizzaRepository.GetByIdAsync(id);
    }


    public async Task<List<Pizza>> GetPizzaAsync()
    {
        return await _pizzaRepository.GetAsync();
    }

    public async Task UpdatePizzaAsync(string id, Pizza pizza)
    {
        await _pizzaRepository.UpdateAsync(id, pizza);
    }
}