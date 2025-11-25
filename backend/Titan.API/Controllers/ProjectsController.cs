using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Titan.API.DTOs;
using Titan.API.Services;
using System.Security.Claims;


namespace Titan.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto request)
    {
        var result = await _projectService.CreateAsync(request);

        if (result == null) return BadRequest("Invalid Proejct data");

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _projectService.GetAllAsync();
        return Ok(result);
    }

    [HttpPost("assign")]

    public async Task<IActionResult> AssignEmployee(AssignProjectDto request)
    {
        var result = await _projectService.AssignEmployeeAsync(request);

        if (!result) return BadRequest("Invalid assignment (Project/User missing or already assigned)");

        return Ok("Employee assigned successfully");
    }

    [HttpPost("unassign")]

    public async Task<IActionResult> RemoveEmployeeAsync(AssignProjectDto request)
    {
        var resposne = await _projectService.RemoveEmployeeAsync(request);

        if (!resposne) return BadRequest("Assignment not found");

        return Ok("Employee removed successfully");
    }


    [HttpGet("my-projects")]
    public async Task<IActionResult> GetMyProjects()
    {

        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized("User ID not found in token.");

        int userId = int.Parse(userIdString);


        var result = await _projectService.GetProjectsForUserAsync(userId);

        return Ok(result);
    }

}