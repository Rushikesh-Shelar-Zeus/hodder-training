using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;
using ContosoPizzaNoSQl.Repositories.Interfaces;
using MongoDB.Driver;

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


    public async Task<List<Pizza>> GetAllPizzaAsync()
    {
        return await _pizzaRepository.GetAllAsync();
    }

    public async Task<Pizza?> UpdatePizzaAsync(string id, Pizza pizza)
    {
        await _pizzaRepository.UpdateAsync(id, pizza);
        return await _pizzaRepository.GetByIdAsync(id);
    }

    public async Task<PagedOutput<Pizza>> GetPizzaAsync(int pageNumber, int pageSize, string sortBy, string order)
    {
        bool ascending = order.Equals("ASC", StringComparison.OrdinalIgnoreCase);
        var sortDefinition = sortBy switch
        {
            "price" => ascending ? Builders<Pizza>.Sort.Ascending(p => p.Price) : Builders<Pizza>.Sort.Descending(p => p.Price),
            "name" => ascending ? Builders<Pizza>.Sort.Ascending(p => p.Name) : Builders<Pizza>.Sort.Descending(p => p.Name),
            _ => Builders<Pizza>.Sort.Ascending(p => p.Name),
        };
        int totalCount = await _pizzaRepository.CountAsync();
        var pizzas = await _pizzaRepository.GetAsync(pageNumber, pageSize, sortDefinition);

        return new PagedOutput<Pizza>(pizzas, totalCount, pageNumber, pageSize);
    }
}