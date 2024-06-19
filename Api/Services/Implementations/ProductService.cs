using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using AutoMapper;
using Models.Dtos;
using Models.Entities;
using Serilog;

namespace Api.Services.Implementations;

public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
    public async Task<ApiResponseDto<IEnumerable<ProductDto>>> GetAllProducts()
    {
        var products = await productRepository.GetAllProducts();
        return new ApiResponseDto<IEnumerable<ProductDto>>(true, "All products found!", products.Select(mapper.Map<ProductDto>).ToList());
    }

    public async Task<ApiResponseDto<IEnumerable<ProductDto>>> GetAllProductsByName(string name)
    {
        var products = await productRepository.GetProductsByName(name);
        return new ApiResponseDto<IEnumerable<ProductDto>>(true, "All products by name found!", products.Select(mapper.Map<ProductDto>).ToList());
    }

    public async Task<ApiResponseDto<IEnumerable<ProductDto>>> GetAllProductsByCategoryAndPrice(string category, decimal price)
    {
        var products = await productRepository.GetProductsByCategoryAndPrice(category, price);
        return new ApiResponseDto<IEnumerable<ProductDto>>(true, "All products by category and price found!", products.Select(mapper.Map<ProductDto>).ToList());
    }

    public async Task<ApiResponseDto<ProductDto?>> GetProductById(Guid id)
    {
        try
        {
            var product = await productRepository.GetProductById(id);
            if (product is null) throw new Exception("ERROR: Product with given ID not found -> " + id);
            return new ApiResponseDto<ProductDto?>(true, "Product with given ID found!",
                mapper.Map<ProductDto>(product));
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return new ApiResponseDto<ProductDto?>(false, e.Message, new ProductDto(Guid.Empty, "", 0.0m, "", "", "", []));
        }
    }

    public async Task AddProduct(AddProductDto productDto)
    {
        try
        {
            if (!ValidateProduct(productDto)) return;
            var images = productDto.Images.Select(imageUrl => new Image { ImageUrl = imageUrl }).ToList();
            var newProduct = new Product
            {
                Title = productDto.Title,
                Price = productDto.Price,
                Description = productDto.Description,
                Category = productDto.Category,
                Sku = productDto.Sku,
                Images = images
            };
            await productRepository.AddProduct(newProduct);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
        }
    }

    public bool ValidateProduct(AddProductDto productDto)
    {
        if(string.IsNullOrEmpty(productDto.Title) || string.IsNullOrWhiteSpace(productDto.Title)) return false;
        if(string.IsNullOrEmpty(productDto.Description) || string.IsNullOrWhiteSpace(productDto.Description) || productDto.Description.Length > 100) return false;
        if(string.IsNullOrEmpty(productDto.Category) || string.IsNullOrWhiteSpace(productDto.Category)) return false;
        if(string.IsNullOrEmpty(productDto.Sku) || string.IsNullOrWhiteSpace(productDto.Sku)) return false;
        if (productDto.Images.Count == 0) return false;
        return true;
    }
}