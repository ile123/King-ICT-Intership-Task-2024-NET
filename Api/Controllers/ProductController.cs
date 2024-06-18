using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService, IExternalApiService externalApiService) : ControllerBase
{
    [HttpGet]
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
            //Logger here
            return StatusCode(500, new ApiResponseDto<IEnumerable<ProductDto>>(false, e.Message, new List<ProductDto>()));
        }
    }

    [HttpGet("fetch-data-external-api")]
    public async Task<ActionResult<ApiResponseDto<string>>> GetDataFromApi(
        [FromQuery] string baseUrl = "",
        [FromQuery] string endpoint = "")
    {
        try
        {
            var externalApiData = await externalApiService.GetDataFromApi(baseUrl, endpoint);
            if (!externalApiData.Any())
                throw new Exception("ERROR: Fetching data from a external API has failed!");

            return Ok(new ApiResponseDto<string>(true, "Data fetched successfully!", "Data stored successfully!"));
        }
        catch (Exception e)
        {
            //Logger here
            return StatusCode(500, new ApiResponseDto<IEnumerable<ProductDto>>(false, e.Message, new List<ProductDto>()));
        }
    }
    
    [HttpGet("category-and-price")]
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
            //Logger here
            return StatusCode(500, new ApiResponseDto<IEnumerable<ProductDto>>(false, e.Message, new List<ProductDto>()));
        }
    }
    
    [HttpGet("name")]
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
            //Logger here
            return StatusCode(500, new ApiResponseDto<IEnumerable<ProductDto>>(false, e.Message, new List<ProductDto>()));
        }
    }
    
    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<ApiResponseDto<IEnumerable<ProductDto>>>> GetAllProductsById(Guid productId)
    {
        try
        {
            var result = await productService.GetProductById(productId);
            if (!result.Success) throw new Exception(result.Message);
            return Ok(result);
        }
        catch (Exception e)
        {
            //Logger here
            return StatusCode(500, new ApiResponseDto<IEnumerable<ProductDto>>(false, e.Message, new List<ProductDto>()));
        }
    }
}