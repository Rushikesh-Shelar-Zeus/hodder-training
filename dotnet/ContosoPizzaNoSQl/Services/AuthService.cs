using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;

namespace ContosoPizzaNoSQl.Services;

public class AuthService
{
    private readonly ICustomerService _customerService;
    private readonly JwtService _jwtService;
    public AuthService(ICustomerService customerService, JwtService jwtService)
    {
        _customerService = customerService;
        _jwtService = jwtService;
    }

    public async Task<string> RegisterAsync(string name, string username, string email, string password, string role = "User")
    {
        try
        {
            Console.WriteLine($"Checking if username {username} is available...");
            var isUsernameAvailable = await _customerService.IsUsernameAvailableAsync(username);
            if (!isUsernameAvailable)
            {
                throw new GraphQLException(
                        ErrorBuilder.New()
                            .SetMessage("Username Already Taken")
                            .SetCode("USERNAME_TAKEN")
                            .Build()
                    );
            }

            Console.WriteLine($"Checking if email {email} is already registered...");
            var existingCustomer = await _customerService.GetCustomerByEmailAsync(email);
            if (existingCustomer != null)
            {
                throw new GraphQLException(
                        ErrorBuilder.New()
                            .SetMessage("User with This Email Already Exits")
                            .SetCode("USER_ALREADY_EXISTS")
                            .Build()
                    );
            }

            var newCustomer = new Customer
            {
                Name = name,
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            Console.WriteLine($"Creating new customer with username {username} and email {email}...");
            await _customerService.CreateCustomerAsync(newCustomer);

            Console.WriteLine($"Customer {username} registered successfully.");
            return _jwtService.GenerateToken(newCustomer.Id, role);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during registration: {ex.Message}");
            throw new GraphQLException(
                ErrorBuilder.New()
                    .SetMessage("Registration Failed")
                    .SetCode("REGISTRATION_FAILED")
                    .Build()
            );
        }
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var customer = await _customerService.GetCustomerByUsernameAsync(username);

        if (customer == null || !BCrypt.Net.BCrypt.Verify(password, customer.PasswordHash))
            throw new Exception("Invalid username or password");

        return _jwtService.GenerateToken(customer.Id, "User");
    }
}

