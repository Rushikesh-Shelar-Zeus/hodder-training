using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;

namespace ContosoPizzaNoSQl.GraphQL.Customers;

[ExtendObjectType(typeof(Mutation))]
public class CustomerMutations
{
    private readonly ICustomerService _customerService;

    public CustomerMutations(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<Customer> CreateCustomer(CreateCustomerInput input)
    {
        var customer = new Customer
        {
            Name = input.Name,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            Address = input.Address
        };

        await _customerService.CreateCustomerAsync(customer);
        return customer;
    }

    public async Task UpdateCustomer(string id, CreateCustomerInput input)
    {
        var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
        if (existingCustomer == null)
        {
            throw new GraphQLException(new Error("Customer not found", "NOT_FOUND"));
        }

        var updatedCustomer = new Customer
        {
            Name = input.Name,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            Address = input.Address
        };

        await _customerService.UpdateCustomerAsync(id, updatedCustomer);
    }

    public async Task<bool> DeleteCustomer(string id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            throw new GraphQLException(new Error("Customer not found", "NOT_FOUND"));
        }

        await _customerService.DeleteCustomerAsync(id);
        return true;
    }
}