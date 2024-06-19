using Models.Dtos;

namespace Api.Services.Interfaces;

public interface IExternalApiService
{
    Task<List<AddProductDto>> GetDataFromApi(string baseUrl, string endpoint);
}