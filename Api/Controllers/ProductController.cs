using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models.Dtos;
using Models.Entities;
using Serilog;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService, IMemoryCache memoryCache) : ControllerBase
{
    /// <summary>Gets all products from the db</summary>  
    /// <returns>List of product dtos</returns>  
    /// <response code="200">If all products where found</response>  
    /// <response code="500">If there was a error during the process of fetching the products from the db</response> 
    [HttpGet]
    [Authorize(Policy = "AuthenticatedOnly")]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductDto>>>> GetAllProducts()
    {
        try
        {
            var result = await productService.GetAllProducts();
            if (!result.Success) throw new Exception(result.Message);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return StatusCode(500, new ApiResponseDto<IEnumerable<ProductDto>>(false, e.Message, new List<ProductDto>()));
        }
    }

    /// <summary>Gets all products from the db by category and price(equal or less)</summary>  
    /// <returns>List of product dtos</returns>  
    /// <response code="200">If all products found</response>  
    /// <response code="500">If there was a error during the process of fetching the products from the db</response> 
    [HttpGet("category-and-price")]
    [Authorize(Policy = "AuthenticatedOnly")]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductDto>>>> GetAllProductsByCategoryAndPrice(
        [FromQuery] string category = "",
        [FromQuery] decimal price = 1000.0m)
    {
        try
        {
            var result = await productService.GetAllProductsByCategoryAndPrice(category, price);
            if (!result.Success) throw new Exception(result.Message);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return StatusCode(500, new ApiResponseDto<IEnumerable<ProductDto>>(false, e.Message, new List<ProductDto>()));
        }
    }

    /// <summary>Gets all products from the db that has the same or similarl name</summary>  
    /// <returns>List of product dtos</returns>  
    /// <response code="200">If all products found</response>  
    /// <response code="500">If there was a error during the process of fetching the products from the db</response> 
    [HttpGet("name")]
    [Authorize(Policy = "AuthenticatedOnly")]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductDto>>>> GetAllProductsByName(
        [FromQuery] string name = "")
    {
        try
        {
            var result = await productService.GetAllProductsByName(name);
            if (!result.Success) throw new Exception(result.Message);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<IEnumerable<ProductDto>>(false, e.Message, new List<ProductDto>()));
        }
    }

    /// <summary>Gets the product information from the db with the given id</summary>  
    /// <returns>Returns a product dto</returns>  
    /// <response code="200">If all products found</response>  
    /// <response code="500">If there was a error during the process of fetching the product from the db</response> 
    [HttpGet("{productId:guid}")]
    [Authorize(Policy = "AuthenticatedOnly")]
    public async Task<ActionResult<ApiResponseDto<ProductDto>>> GetProductById(Guid productId)
    {
        try{
            var result = await productService.GetProductById(productId);
            if (!result.Success) throw new Exception(result.Message);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<IEnumerable<ProductDto>>(false, e.Message, new List<ProductDto>()));
        }
    }
}