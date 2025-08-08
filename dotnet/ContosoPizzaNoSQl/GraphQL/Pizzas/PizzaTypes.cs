namespace ContosoPizzaNoSQl.GraphQL.Pizzas;

public class CreatePizzaInput
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsGlutenFree { get; set; }
}
