namespace ContosoPizza.Dtos.Pizza;

public class PizzaDto
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsGlutenFree { get; set; }
}