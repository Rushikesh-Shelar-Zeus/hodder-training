using ContosoPizzaNoSQl.Models;

namespace ContosoPizzaNoSQl.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAsync();
    Task<Customer?> GetByIdAsync(string id);
    Task CreateAsync(Customer customer);
    Task UpdateAsync(string id, Customer customer);
    Task DeleteAsync(string id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByUsernameAsync(string username);
}