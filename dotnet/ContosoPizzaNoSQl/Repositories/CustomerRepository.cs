using ContosoPizzaNoSQl.Configuration;
using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ContosoPizzaNoSQl.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IMongoCollection<Customer> _customer;

    public CustomerRepository(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _customer = database.GetCollection<Customer>("Customers");
    }

    public async Task CreateAsync(Customer customer)
    {
        await _customer.InsertOneAsync(customer);
    }

    public async Task DeleteAsync(string id)
    {
        await _customer.DeleteOneAsync(c => c.Id == id);
    }

    public async Task<List<Customer>> GetAsync()
    {
        return await _customer.Find(_ => true).ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(string id)
    {
        return await _customer.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(string id, Customer customer)
    {
        await _customer.UpdateOneAsync(c => c.Id == id, Builders<Customer>.Update
        .Set(c => c.Name, customer.Name)
        .Set(c => c.Email, customer.Email)
        .Set(c => c.PhoneNumber, customer.PhoneNumber)
        .Set(c => c.Address, customer.Address));
    }

}