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
public class ProductController(IProductService productService, IExternalApiService externalApiService, IMemoryCache memoryCache) : ControllerBase
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

    /// <summary>Gets all products from a external api and stores them in the db</summary>  
    /// <returns>A string that confirms that products where retrived from the external api and stored in the db</returns>  
    /// <response code="200">If the products where fetched and stored in the db</response>  
    /// <response code="500">If there was a error during the process of fetching the products or storing them in the db</response> 
    [HttpGet("fetch-data-from-api")]
    [Authorize(Policy = "AuthenticatedOnly")]
    public async Task<ActionResult<ApiResponseDto<string>>> GetDataFromApi(
        [FromQuery] string baseUrl = "",
        [FromQuery] string endpoint = "")
    {
        try
        {
            var externalApiData = await externalApiService.GetDataFromApi(baseUrl, endpoint);
            if (!externalApiData.Any())
                throw new Exception("ERROR: Fetching data from a external API has failed!");
            foreach (var productDto in externalApiData)
            {
                await productService.AddProduct(productDto);
            }
            return Ok(new ApiResponseDto<string>(true, "Data fetched successfully!", "Data stored successfully!"));
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
            var cacheKey = $"ProductsByCategoryAndPrice_{category}_{price}";
            if (memoryCache.TryGetValue(cacheKey, out List<Product>? cachedProducts))
            {
                return Ok(cachedProducts);
            }

            var result = await productService.GetAllProductsByCategoryAndPrice(category, price);
            if (!result.Success) throw new Exception(result.Message);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            memoryCache.Set(cacheKey, result, cacheOptions);

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
            var cacheKey = $"ProductsByName_{name}";
            if (memoryCache.TryGetValue(cacheKey, out List<Product>? cachedProducts))
            {
                return Ok(cachedProducts);
            }

            var result = await productService.GetAllProductsByName(name);
            if (!result.Success) throw new Exception(result.Message);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            memoryCache.Set(cacheKey, result, cacheOptions);

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
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductDto>>>> GetAllProductsById(Guid productId)
    {
        try
        {
            var cacheKey = $"ProductById_{productId}";
            if (memoryCache.TryGetValue(cacheKey, out List<Product>? cachedProducts))
            {
                return Ok(cachedProducts);
            }

            var result = await productService.GetProductById(productId);
            if (!result.Success) throw new Exception(result.Message);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            memoryCache.Set(cacheKey, result, cacheOptions);

            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, new ApiResponseDto<IEnumerable<ProductDto>>(false, e.Message, new List<ProductDto>()));
        }
    }
}