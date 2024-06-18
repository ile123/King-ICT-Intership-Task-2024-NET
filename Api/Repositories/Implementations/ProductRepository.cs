using Api.Repositories.Interfaces;
using Models.Entities;

namespace Api.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    public Task<List<Product>> GetAllProducts()
    {
        throw new NotImplementedException();
    }

    public Task<List<Product>> GetProductsByName(string name)
    {
        throw new NotImplementedException();
    }

    public Task<List<Product>> GetProductsByCategoryAndPrice(string category, decimal price)
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetProductById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task AddProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProduct(Guid id)
    {
        throw new NotImplementedException();
    }
}