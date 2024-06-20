using Models.Dtos;

namespace Api.Services.Interfaces;

public interface IAuthService
{
    ApiResponseDto<string> GetToken();
    string CreateToken();
}