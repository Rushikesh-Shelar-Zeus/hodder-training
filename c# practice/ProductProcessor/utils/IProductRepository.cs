using ProductProcessor.Products;

namespace ProductProcessor.Interfaces;

interface IProductRepository
{
    Task<Product> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetProductByCategoryAsync(string category);
}