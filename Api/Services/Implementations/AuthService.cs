using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Models.Dtos;
using Models.Entities;
using Models.Enums;

namespace Api.Services.Implementations;

public class AuthService(IAuthRepository authRepository, IConfiguration configuration) : IAuthService
{
    public async Task<ApiResponseDto<string>> Register(RegisterDto registerDto)
    {
        if (await authRepository.GetUserByEmail(registerDto.Email) is not null)  new ApiResponseDto<string>(true, "User registration failed.", "User registration failed!");
        var user = new User
        {
            Email = registerDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            FullName = registerDto.FullName,
            Role = Role.User
        };
        await authRepository.AddUser(user);
        return new ApiResponseDto<string>(true, "User registered.", "User registered sucsefully!");
    }

    public async Task<ApiResponseDto<string>> Login(LoginDto loginDto)
    {
        var user = await authRepository.GetUserByEmail(loginDto.Email);
        if (user is null)
        {
            return new ApiResponseDto<string>(false, "ERROR: User with given email dose not exist!", "");
        }
        return !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password) 
            ? new ApiResponseDto<string>(false, "ERROR: Given password is not the same !", "")
            : new ApiResponseDto<string>(true, "User authenticated successfully!", CreateToken(user));
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var appSettingsToken = configuration.GetSection("AppSettings:Token").Value;

        if (appSettingsToken is null) throw new Exception("ERROR: Token is null!");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<ApiResponseDto<string>> CreateTestUsers()
    {
        if (await authRepository.GetUserByEmail("test_user@test.com") is null)
        {
            var user = new User
            {
                Email = "test_user@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("test"),
                FullName = "Test User",
                Role = Role.User
            };
            await authRepository.AddUser(user);
        }

        if (await authRepository.GetUserByEmail("test_admin@test.com") is null)
        {
            var admin = new User
            {
                Email = "test_admin@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("test"),
                FullName = "Test Admin",
                Role = Role.Admin
            };
            await authRepository.AddUser(admin);
        }
        return new ApiResponseDto<string>(true, "Test users created.", "All test users have been created!");
    }
}