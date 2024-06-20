using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Generates and returns a JWT token</summary>  
    /// <returns>JWT token</returns>
    /// <remarks>
    /// This is a simple authentication and authorization system,
    /// it can be expanded upon in the future to be more secure and complex
    /// </remarks>  
    /// <response code="200">If JWT token was created successfully</response>  
    [HttpPost("get-token")]
    public ActionResult<ApiResponseDto<string>> GetToken()
    {
        return Ok(authService.GetToken());
    }
}