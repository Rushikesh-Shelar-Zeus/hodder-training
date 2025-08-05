
using ContosoPizza.Dtos.Customers;

namespace ContosoPizza.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateCustomerDto dto);
    Task<bool> UpdateAsync(int id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(int id);
}