using Models.Entities;

namespace Api.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<IEnumerable<Product>> GetProductsByName(string title);
    Task<IEnumerable<Product>> GetProductsByCategoryAndPrice(string category, decimal price);
    Task<Product?> GetProductById(Guid id);
    Task AddProduct(Product product);
    Task<bool> DoesProductWithSkuExist(string sku);
}