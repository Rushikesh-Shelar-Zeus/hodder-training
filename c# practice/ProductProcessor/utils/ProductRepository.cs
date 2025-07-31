using ProductProcessor.Products;
using ProductProcessor.Interfaces;

namespace ProductProcessor.Products.Repository;

public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products =
    [
        new Product( 1, "Laptop", 999.99m, "Electronics"),
        new Product ( 2, "TV", 650.00m, "Electronics" ),
        new Product ( 3, "Blender",80.00m,"Home" ),
        new Product ( 4, "Smartphone", 1200.00m,"Electronics" )
    ];

    public Task<Product> GetProductByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id {id} not found.");
        }
        return Task.FromResult(product);
    }

    public Task<IEnumerable<Product>> GetProductByCategoryAsync(string category)
    {
        var result = _products
            .Where(p => p.Category == category && p.Price > 500)
            .ToList();

        return Task.FromResult<IEnumerable<Product>>(result);    
    }

}