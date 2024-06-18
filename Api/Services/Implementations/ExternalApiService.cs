using Api.Services.Interfaces;
using Models.Dtos;

namespace Api.Services.Implementations;

public class ExternalApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IExternalApiService
{
    public async Task<IEnumerable<ProductDto>> GetDataFromApi(string baseUrl, string endpoint)
    {
        var apiBaseUrl = configuration.GetValue<string>("DataSources:" + baseUrl + ":" + "BaseUrl");
        var apiEndpoint = configuration.GetValue<string>("DataSources:" + baseUrl + ":" + endpoint);

        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(apiBaseUrl);
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        
        var response = await httpClient.GetAsync(apiEndpoint);
        if (!response.IsSuccessStatusCode) return new List<ProductDto>();
        //This dosent work
        var responseBody = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
        return responseBody ?? [];
    }
}