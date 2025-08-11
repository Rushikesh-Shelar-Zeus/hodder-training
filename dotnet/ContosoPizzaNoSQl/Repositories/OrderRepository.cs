using MongoDB.Driver;
using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using ContosoPizzaNoSQl.Configuration;

namespace ContosoPizzaNoSQl.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orders;
    public OrderRepository(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _orders = database.GetCollection<Order>("Orders");
    }
    public async Task CreateAsync(Order order)
    {
        try
        {
            await _orders.InsertOneAsync(order);
            var orderItems = order.OrderItems.Select(item => new OrderItem
            {
                PizzaId = item.PizzaId,
                Quantity = item.Quantity,
                PizzaName = item.PizzaName,
                UnitPrice = item.UnitPrice,
                IsGlutenFree = item.IsGlutenFree
            }).ToList();
            
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task DeleteAsync(string id)
    {
        try
        {
            await _orders.DeleteOneAsync(o => o.Id == id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Order>> GetAsync()
    {
        try
        {
            return await _orders.Find(_ => true).ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<List<Order>> GetByCustomerIdAsync(string customerId)
    {
        try
        {
            return _orders.Find(o => o.CustomerId == customerId).ToListAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Order?> GetByIdAsync(string id)
    {
        try
        {
            return await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateAsync(string id, Order updatedOrder)
    {
        try
        {
            await _orders.ReplaceOneAsync(o => o.Id == id, updatedOrder);
        }
        catch (Exception)
        {
            throw;
        }
    }
}