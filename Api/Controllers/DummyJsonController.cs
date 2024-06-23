using Api.Services.Implementations;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Serilog;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DummyJsonController : ControllerBase
{

    private readonly IProductService productService;
    private readonly IExternalApiService externalApiService;

    public DummyJsonController(IProductService productService, DummyJsonService dummyJsonService) 
    {
        this.productService = productService;
        this.externalApiService = dummyJsonService;
    }

    /// <summary>Gets all products from a external api and stores them in the db</summary>  
    /// <returns>A string that confirms that products where retrived from the external api and stored in the db</returns>  
    /// <response code="200">If the products where fetched and stored in the db</response>  
    /// <response code="500">If there was a error during the process of fetching the products or storing them in the db</response> 
    [HttpGet("")]
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
}