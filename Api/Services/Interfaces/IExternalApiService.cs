using Models.Dtos;

namespace Api.Services.Interfaces;

public interface IExternalApiService
{
    Task<IEnumerable<ProductDto>> GetDataFromApi(string baseUrl, string endpoint);
}