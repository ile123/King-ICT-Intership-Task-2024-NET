using Api.Services.Interfaces;

namespace Api.Tests;

public class ExternalApiTests(IExternalApiService externalApiService)
{
    private readonly IExternalApiService _externalApiService = externalApiService;

    [Fact]
    public async void It_Should_Get_Data_From_External_Api()
    {
        var externalApiData = await _externalApiService.GetDataFromApi("DummyJson", "Products");
        Assert.True(externalApiData.Any(), "External api has retrieved data and converted it into the required product dtos.");
    }
}