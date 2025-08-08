using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;

namespace ContosoPizzaNoSQl.GraphQL.Pizzas;

[ExtendObjectType(typeof(Query))]

public class PizzaQueries
{
    public async Task<List<Pizza>> GetPizzas([Service] IPizzaService pizzaService)
    {
        return await pizzaService.GetPizzaAsync();
    }


    public async Task<Pizza?> GetPizzaById(string id, [Service] IPizzaService pizzaService)
    {
        return await pizzaService.GetPizzaByIdAsync(id);
    }
}