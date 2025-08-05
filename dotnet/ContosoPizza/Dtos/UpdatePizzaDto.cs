using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Dtos;

public class UpdatePizzaDto
{
    [Required]
    public string? Name { get; set; } = string.Empty;

    [Required, Range(0.01, 10000)]
    public decimal Price { get; set; }

    public bool IsGlutenFree { get; set; } = false;
}