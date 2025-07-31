namespace ProductProcessor.Products;

public class Product(int id, string name, decimal price, string category)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
    public string Category { get; set; } = category;
}