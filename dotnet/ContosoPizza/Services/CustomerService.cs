
using ContosoPizza.Models;
using ContosoPizza.Dtos.Customers;
using ContosoPizza.Services.Interfaces;
using ContosoPizza.Repositories.Interfaces;

namespace ContosoPizza.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        try
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all customers");
            throw;
        }
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer is null) return null;

            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer with ID {id}", id);
            return null;
        }
    }

    public async Task<int> CreateAsync(CreateCustomerDto dto)
    {
        try
        {
            var newCustomer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };
            return await _customerRepository.CreateAsync(newCustomer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new Customer");
            return -1;
        }
    }

    public async Task<bool> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        try
        {
            var customer = new Customer
            {
                Id = id,
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };

            return await _customerRepository.UpdateAsync(id, customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Customer with ID {id}", id);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            return await _customerRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Customer with ID {id}", id);
            return false;
        }
    }
}