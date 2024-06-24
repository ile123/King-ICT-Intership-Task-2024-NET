using Models.Dtos;
using Models.Entities;

namespace Api.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResponseDto<string>> Register(RegisterDto registerDto);
    Task<ApiResponseDto<string>> Login(LoginDto loginDto);
    string CreateToken(User user);
    Task<ApiResponseDto<string>> CreateTestUsers();
}