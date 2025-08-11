using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Repositories.Interfaces;
using ContosoPizzaNoSQl.Services.Interfaces;

namespace ContosoPizzaNoSQl.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task CreateCustomerAsync(Customer customer)
    {
        await _customerRepository.CreateAsync(customer);
    }

    public async Task DeleteCustomerAsync(string id)
    {
        await _customerRepository.DeleteAsync(id);
    }

    public async Task<List<Customer>> GetCustomerAsync()
    {
        return await _customerRepository.GetAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(string id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<Customer?> UpdateCustomerAsync(string id, Customer customer)
    {
        await _customerRepository.UpdateAsync(id, customer);
        return await _customerRepository.GetByIdAsync(id);
    }
}