
using ContosoPizzaNoSQl.Services;

namespace ContosoPizzaNoSQl.GraphQL.Auth;

[ExtendObjectType(typeof(Mutation))]
public class AuthMutations
{
    public async Task<string> Register(
        string name,
        string username,
        string email,
        string password,
        [Service] AuthService authService
    )
    {
        return await authService.RegisterAsync(name, username, email, password);
    }

    public async Task<string> Login(
        string username,
        string password,
        [Service] AuthService authService
    )
    {
        return await authService.LoginAsync(username, password);
    }
}