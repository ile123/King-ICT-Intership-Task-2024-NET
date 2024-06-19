using Api.Services.Interfaces;

namespace Api.Tests;

public class ProductTests
{
    private readonly IProductService _productService;

    public ProductTests(IProductService productService)
    {
        _productService = productService;
    }

    [Fact]
    public async Task It_Should_Get_All_Products()
    {
        var products = await _productService.GetAllProducts();
        Assert.True(products.Result.Any(), "Products retrieved from db successfully!");
    }

    [Fact]
    public async Task It_Should_Get_Product_By_Id()
    {
        var products = await _productService.GetAllProducts();
        var productFoundById = await _productService.GetProductById(products.Result.First().Id);
        Assert.NotNull(productFoundById);
        Assert.Equal(products.Result.First().Id, productFoundById.Result.Id);
    }
}