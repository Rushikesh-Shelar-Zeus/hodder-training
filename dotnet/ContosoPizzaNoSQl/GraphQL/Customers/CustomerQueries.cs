using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;

namespace ContosoPizzaNoSQl.GraphQL.Customers;

[ExtendObjectType(typeof(Query))]
public class CustomerQueries
{
    public async Task<List<Customer>> GetCustomers([Service] ICustomerService customerService)
    {
        return await customerService.GetCustomerAsync();
    }

    public async Task<Customer?> GetCustomerById(string id, [Service] ICustomerService customerService)
    {
        return await customerService.GetCustomerByIdAsync(id);
    }
}