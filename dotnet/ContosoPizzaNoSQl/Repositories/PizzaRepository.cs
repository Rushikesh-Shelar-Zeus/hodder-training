
using ContosoPizzaNoSQl.Configuration;
using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ContosoPizzaNoSQl.Repositories;

public class PizzaRepository : IPizzaRepository
{
    private readonly IMongoCollection<Pizza> _pizzas;

    public PizzaRepository(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _pizzas = database.GetCollection<Pizza>("Pizzas");
    }

    public async Task<List<Pizza>> GetAsync()
    {
        return await _pizzas.Find(_ => true).ToListAsync();
    }

    public async Task<Pizza?> GetByIdAsync(string str)
    {
        return await _pizzas.Find(p => p.Id == str).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Pizza pizza)
    {
        await _pizzas.InsertOneAsync(pizza);
    }

    public async Task UpdateAsync(string id, Pizza pizza)
    {
        await _pizzas.UpdateOneAsync(p => p.Id == id, Builders<Pizza>.Update
            .Set(p => p.Name, pizza.Name)
            .Set(p => p.IsGlutenFree, pizza.IsGlutenFree)
            .Set(p => p.Price, pizza.Price));
    }

    public Task DeleteAsync(string id)
    {
        return _pizzas.DeleteOneAsync(p => p.Id == id);
    }
}