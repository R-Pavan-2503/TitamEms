using Titan.API.DTOs;
using Titan.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Titan.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var user = await _authService.RegisterAsync(request);
        if (user == null) return BadRequest("Email alredy exisits");

        return Ok(user);

    }

    [HttpPost("login")]

    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var token = await _authService.LoginAsync(request);

        if (token == null) return Unauthorized("Invalid credentilos");

        return Ok(new { token = token });
    }


}