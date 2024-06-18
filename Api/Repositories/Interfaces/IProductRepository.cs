using Models.Dtos;
using Models.Entities;

namespace Api.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllProducts();
    Task<List<Product>> GetProductsByName(string name);
    Task<List<Product>> GetProductsByCategoryAndPrice(string category, decimal price);
    Task<Product?> GetProductById(Guid id);
    Task AddProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(Guid id);
}