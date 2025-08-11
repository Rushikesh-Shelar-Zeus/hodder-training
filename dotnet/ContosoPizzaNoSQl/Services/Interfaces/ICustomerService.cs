using ContosoPizzaNoSQl.Models;

namespace ContosoPizzaNoSQl.Services.Interfaces;

public interface ICustomerService
{
    Task<List<Customer>> GetCustomerAsync();
    Task<Customer?> GetCustomerByIdAsync(string id);
    Task CreateCustomerAsync(Customer customer);
    Task<Customer?> UpdateCustomerAsync(string id, Customer customer);
    Task DeleteCustomerAsync(string id);
}