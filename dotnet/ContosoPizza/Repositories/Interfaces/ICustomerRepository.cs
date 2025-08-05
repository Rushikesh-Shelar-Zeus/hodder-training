using ContosoPizza.Models;

namespace ContosoPizza.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task<int> CreateAsync(Customer customer);
    Task<bool> UpdateAsync(int id, Customer customer);
    Task<bool> DeleteAsync(int id);
}