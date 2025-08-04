using ContosoPizza.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ContosoPizza.Repositories;

namespace ContosoPizza.Services;

public class PizzaService : IPizzaService
{
    private readonly IPizzaRepository _pizzaRepository;
    public PizzaService(IPizzaRepository pizzaRepository, ILogger<PizzaService> logger)
    {
        _pizzaRepository = pizzaRepository;
    }

    public async Task<IEnumerable<Pizza>> GetAllPizzasAsync() => await _pizzaRepository.GetAllAsync();

    public async Task<Pizza?> GetPizzaByIdAsync(int id) => await _pizzaRepository.GetByIdAsync(id);

    public async Task<int> AddPizzaAsync(Pizza pizza) => await _pizzaRepository.AddAsync(pizza);
    public async Task UpdatePizzaAsync(int id, Pizza pizza) => await _pizzaRepository.UpdateAsync(id, pizza);

    public async Task DeletePizzaAsync(int id) => await _pizzaRepository.DeleteAsync(id);
}