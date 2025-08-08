namespace ContosoPizzaNoSQl.GraphQL.Customers;

public class CreateCustomerInput
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}
