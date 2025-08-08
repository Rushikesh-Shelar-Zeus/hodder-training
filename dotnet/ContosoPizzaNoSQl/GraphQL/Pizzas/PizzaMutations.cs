using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;

namespace ContosoPizzaNoSQl.GraphQL.Pizzas;

[ExtendObjectType(typeof(Mutation))]
public class PizzaMutations
{
    public async Task<Pizza> CreatePizza(CreatePizzaInput input, [Service] IPizzaService pizzaService)
    {
        var pizza = new Pizza
        {
            Name = input.Name,
            Price = input.Price,
            IsGlutenFree = input.IsGlutenFree
        };

        await pizzaService.CreatePizzaAsync(pizza);
        return pizza;
    }

    public async Task UpdatePizza(string id, CreatePizzaInput input, [Service] IPizzaService pizzaService)
    {

        var existing = await pizzaService.GetPizzaByIdAsync(id);
        if (existing == null)
        {
            throw new GraphQLException(new Error("Pizza not found", "NOT_FOUND"));
        }

        var updatedPizza = new Pizza
        {
            Name = input.Name,
            Price = input.Price,
            IsGlutenFree = input.IsGlutenFree
        };

        await pizzaService.UpdatePizzaAsync(id, updatedPizza);
    }

    public async Task<bool> DeletePizza(string id, [Service] IPizzaService pizzaService)
    {
        var existing = await pizzaService.GetPizzaByIdAsync(id);
        if (existing == null)
        {
            throw new GraphQLException(new Error("Pizza not found", "NOT_FOUND"));
        }

        await pizzaService.DeletePizzaAsync(id);
        return true;
    }
}