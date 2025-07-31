using ProductProcessor.Products.Repository;

namespace ProductProcessor.Program;

public class Program
{
    public static async Task Main(String[] args)
    {
        ProductRepository repository = new();

        Console.WriteLine("Fetching product in electronics category with price greater than 500...");

        var products = await repository.GetProductByCategoryAsync("Electronics");

        foreach (var product in products)
        {
            Console.WriteLine($"The {product.Category} Product {product.Name} has a Price of {product.Category}");
        }

        Console.WriteLine("\nFetching product with ID = 3...\n");

        var singleProduct = await repository.GetProductByIdAsync(3);
        Console.WriteLine($"{singleProduct.Name} - ${singleProduct.Price}");
    }
}