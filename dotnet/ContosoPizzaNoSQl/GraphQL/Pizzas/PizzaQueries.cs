using ContosoPizzaNoSQl.GraphQL.SortTypes;
using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Services.Interfaces;

namespace ContosoPizzaNoSQl.GraphQL.Pizzas;

[ExtendObjectType(typeof(Query))]

public class PizzaQueries
{
    public async Task<PagedOutput<Pizza>> GetPizzas(PizzaPagedInput input, [Service] IPizzaService pizzaService)
    {
        var pagedResult = await pizzaService.GetPizzaAsync(pageNumber: input.PageNumber,
                                                          pageSize: input.PageSize,
                                                          sortBy: input.SortBy.ToString().ToLower(),
                                                          order: input.Order.ToString().ToLower());
        return pagedResult;
    }

    public async Task<List<Pizza>> GetAllPizzas([Service] IPizzaService pizzaService)
    {
        return await pizzaService.GetAllPizzaAsync();
    }

    public async Task<Pizza?> GetPizzaById(string id, [Service] IPizzaService pizzaService)
    {
        return await pizzaService.GetPizzaByIdAsync(id);
    }
}