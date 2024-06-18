using Api.Data;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Api.Repositories.Implementations;

public class ProductRepository(AppDbContext appDbContext) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await appDbContext
            .Products
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        return await appDbContext
            .Products
            .Where(x => x.Name == name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAndPrice(string category, decimal price)
    {
        return await appDbContext
            .Products
            .Where(x => x.Category == category && x.Price >= price)
            .ToListAsync();
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        return await appDbContext
            .Products
            .FindAsync(id);
    }

    public async Task AddProduct(Product product)
    {
        if(await DoesProductWithSkuExist(product.Sku)) return;
        await appDbContext.Products.AddAsync(product);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<bool> DoesProductWithSkuExist(string sku)
    {
        return await appDbContext.Products.FirstOrDefaultAsync(x => x.Sku == sku) is not null;
    }
}