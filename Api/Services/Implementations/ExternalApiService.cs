using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Services.Interfaces;
using Models.Dtos;
using Serilog;

namespace Api.Services.Implementations;

public class ExternalApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IExternalApiService
{
    public async Task<List<AddProductDto>> GetDataFromApi(string baseUrl, string endpoint)
    {
        try
        {
            var apiBaseUrl = configuration.GetValue<string>("DataSources:" + baseUrl + ":" + "BaseUrl");
            var apiEndpoint = configuration.GetValue<string>("DataSources:" + baseUrl + ":" + endpoint);

            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(apiBaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(30);
                
            var response = await httpClient.GetAsync(apiEndpoint);
            if (!response.IsSuccessStatusCode) return [];
            var jsonString = await response.Content.ReadAsStringAsync();
            var deserializedJson = JsonSerializer.Deserialize<Products>(jsonString);
            var result = deserializedJson.ProductsDtos
                .Select(x => new AddProductDto(x.Title, x.Price, x.Description, x.Category, x.Sku, x.Images))
                .ToList();
            return await Task.FromResult(result);
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return await Task.FromResult(new List<AddProductDto>());
        }
        
    }

    private class ProductJsonDto
    {
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("price")]       public decimal Price { get; set; }      
        [JsonPropertyName("description")] public string Description { get; set; } 
        [JsonPropertyName("category")]    public string Category { get; set; }    
        [JsonPropertyName("sku")]         public string Sku { get; set; }         
        [JsonPropertyName("images")]      public List<string> Images { get; set; }
    }

    private class Products
    {
        [JsonPropertyName("products")] 
        public List<ProductJsonDto>? ProductsDtos { get; set; }
    }
}