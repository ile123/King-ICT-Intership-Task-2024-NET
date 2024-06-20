using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Models.Dtos;

namespace Api.Services.Implementations;

public class AuthService(IConfiguration configuration) : IAuthService
{
    public ApiResponseDto<string> GetToken()
    {
        return new ApiResponseDto<string>(true, "Token created successfully!", CreateToken());
    }
    
    public string CreateToken()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "test")
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
}