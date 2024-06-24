using Api.Data;
using Api.Repositories.Implementations;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Api.Tests;

public class ProductTests : IDisposable
{

    private readonly AppDbContext _context;
    private readonly IProductRepository _repository;

    public ProductTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost; Database=product_db_net; Username=root; Password=root")
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();

        _repository = new ProductRepository(_context);
    }

    [Fact]
    public async Task It_Should_Get_All_Products()
    {
        _context.Products.AddRange(Enumerable.Range(1, 5).Select(x => new Product
        {
            Title = x.ToString(),
            Price = 2.2m,
            Description = x.ToString(),
            Category = x.ToString(),
            Sku = x.ToString(),
            Images = new List<Image>
            {
                new()
                {
                    ImageUrl = x.ToString()
                }
            }
        }));
        await _context.SaveChangesAsync();

        var products = await _repository.GetAllProducts();
        Assert.NotEmpty(products);
    }

    [Fact]
    public async Task It_Should_Get_Product_By_Id()
    {
        _context.Products.AddRange(Enumerable.Range(1, 5).Select(x => new Product
        {
            Title = "Test Title - " + x,
            Price = 2.2m,
            Description = "Test Description - " + x,
            Category = "Test Category - " + x,
            Sku = "Test Sku - " + x,
            Images = new List<Image>
            {
                new()
                {
                    ImageUrl = x.ToString()
                }
            }
        }));
        await _context.SaveChangesAsync();

        var products = await _repository.GetAllProducts();
        var product = await _repository.GetProductById(products.First().Id);
        Assert.NotNull(product);
        Assert.Equal(product.Id, products.First().Id);
    }

    [Fact]
    public async Task It_Should_Get_Products_By_Name()
    {
        _context.Products.AddRange(Enumerable.Range(1, 5).Select(x => new Product
        {
            Title = "Test Title - " + x,
            Price = 2.2m,
            Description = "Test Description - " + x,
            Category = "Test Category - " + x,
            Sku = "Test Sku - " + x,
            Images = new List<Image>
            {
                new()
                {
                    ImageUrl = x.ToString()
                }
            }
        }));
        await _context.SaveChangesAsync();

        var products = await _repository.GetProductsByName("test");
        Assert.NotEmpty(products);
    }

    [Fact]
    public async Task It_Should_Get_Products_By_Category_And_Price()
    {
        _context.Products.AddRange(Enumerable.Range(1, 5).Select(x => new Product
        {
            Title = "Test Title - " + x,
            Price = 2.2m,
            Description = "Test Description - " + x,
            Category = "Category - " + x,
            Sku = "Test Sku - " + x,
            Images = new List<Image>
            {
                new()
                {
                    ImageUrl = x.ToString()
                }
            }
        }));
        await _context.SaveChangesAsync();

        var products = await _repository.GetProductsByCategoryAndPrice("Category - 1", 4.2m);
        Assert.NotEmpty(products);
        Assert.Single(products.ToList());
    }
    
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}