using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Authenticates a user with the given request</summary>  
    /// <returns>JWT token</returns>
    /// <response code="200">If the user was authenticated successfully, then it returns a JWT token</response>  
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponseDto<string>>> Login(LoginDto loginDto)
    {
        return Ok(await authService.Login(loginDto));
    }
    
    /// <summary>Authenticates a user with the given request</summary>  
    /// <returns>JWT token</returns>
    /// <response code="200">If the user was authenticated successfully, then it returns a JWT token</response>  
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponseDto<string>>> Register(RegisterDto registerDto)
    {
        await authService.Register(registerDto);
        return Ok(new ApiResponseDto<string>(true, "Registration complete!",
            "User registration has been successful."));
    }
    
    /// <summary>Creates test users</summary>  
    /// <returns>String that confirms that the users where created</returns>
    /// <response code="200">If the users where created</response>  
    [HttpPost("create-test-users")]
    public async Task<ActionResult<ApiResponseDto<string>>> CreateTestUsers() 
    {
        await authService.CreateTestUsers();
        return Ok(new ApiResponseDto<string>(true, "Users created!", "Test users have been successfully created!"));
    }
}