using Models.Dtos;
using Models.Entities;

namespace Api.Services.Interfaces;

public interface IProductService
{
    Task<ApiResponseDto<IEnumerable<ProductDto>>> GetAllProducts();
    
    Task<ApiResponseDto<IEnumerable<ProductDto>>> GetAllProductsByName(string name);

    Task<ApiResponseDto<IEnumerable<ProductDto>>> GetAllProductsByCategoryAndPrice(string category, decimal price);

    Task<ApiResponseDto<ProductDto?>> GetProductById(Guid id);

    Task AddProduct(AddProductDto productDto);
    bool ValidateProduct(AddProductDto productDto);
    
}