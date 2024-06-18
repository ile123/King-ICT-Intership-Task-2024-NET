using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using AutoMapper;
using Models.Dtos;
using Models.Entities;

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
            //Logger here
            return new ApiResponseDto<ProductDto?>(false, e.Message, new ProductDto(Guid.Empty, "", 0.0m, "", "", "", []));
        }
    }

    public async Task<ApiResponseDto<Guid>> AddProduct(AddProductDto productDto)
    {
        try
        {
            if (!ValidateProduct(productDto))
                throw new Exception("ERROR: Invalid product tried to be saved to the db!");
            var images = productDto.Images.Select(imageUrl => new Image { ImageUrl = imageUrl }).ToList();
            var newProduct = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                Category = productDto.Category,
                Images = images
            };
            await productRepository.AddProduct(newProduct);
            return new ApiResponseDto<Guid>(true, "Product created successfully!", newProduct.Id);
        }
        catch (Exception e)
        {
            //Logger here
            return new ApiResponseDto<Guid>(false, e.Message, Guid.Empty);
        }
    }

    public bool ValidateProduct(AddProductDto productDto)
    {
        if(string.IsNullOrEmpty(productDto.Name) || string.IsNullOrWhiteSpace(productDto.Name)) return false;
        if(string.IsNullOrEmpty(productDto.Description) || string.IsNullOrWhiteSpace(productDto.Description)) return false;
        if(string.IsNullOrEmpty(productDto.Category) || string.IsNullOrWhiteSpace(productDto.Category)) return false;
        if(string.IsNullOrEmpty(productDto.Sku) || string.IsNullOrWhiteSpace(productDto.Sku)) return false;
        if (productDto.Images.Count == 0) return false;
        return true;
    }
}