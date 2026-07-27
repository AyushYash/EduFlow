using Microsoft.AspNetCore.Mvc;
using EduFlow.DTOs;
using EduFlow.Services.Interfaces;

namespace EduFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authservice;

    public AuthController(IAuthService authService)
    {
        _authservice = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var result =  await _authservice.RegisterAsync(dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message});
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authservice.LoginAsync(dto);
        if (result == null)
        {
            return Unauthorized(new {message = "Invalid Email or Password."});
        }
        return Ok(result);
    }
}