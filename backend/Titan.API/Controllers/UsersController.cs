using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Titan.API.DTOs;
using Titan.API.Services;

namespace Titan.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpPut("me/username")]
    public async Task<IActionResult> UpdateMyUsername([FromBody] UpdateUsernameDto request)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString == null)
            return Unauthorized("Invalid user token");

        int userId = int.Parse(userIdString);

        var updated = await _service.UpdateUsernameAsync(userId, request.UserName);

        if (!updated) return BadRequest("Unable to update the UserName");

        return Ok("User Name Updated  ");

    }

    [Authorize(Roles = "Admin")]
    [HttpGet("employees")]

    public async Task<IActionResult> GetAllEMployees()
    {
        var employees = await _service.GetAllEmployeesAsync();
        return Ok(employees);
    }
}