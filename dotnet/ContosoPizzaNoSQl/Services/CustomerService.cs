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
        try
        {
            return await _customerRepository.GetAsync();
        }catch (Exception ex)
        {
            Console.WriteLine($"Error fetching customers: {ex.Message}");
            throw new GraphQLException(
                ErrorBuilder.New()
                    .SetMessage("Failed to fetch customers")
                    .SetCode("FETCH_CUSTOMERS_FAILED")
                    .Build()
            );
        }
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        return await _customerRepository.GetByEmailAsync(email);
    }

    public async Task<Customer?> GetCustomerByIdAsync(string id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<Customer?> GetCustomerByUsernameAsync(string username)
    {
        return await _customerRepository.GetByUsernameAsync(username);
    }

    public async Task<bool> IsUsernameAvailableAsync(string username)
    {
        var customer = await _customerRepository.GetByUsernameAsync(username);
        return customer == null;
    }

    public async Task<Customer?> UpdateCustomerAsync(string id, Customer customer)
    {
        await _customerRepository.UpdateAsync(id, customer);
        return await _customerRepository.GetByIdAsync(id);
    }
}