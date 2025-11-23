using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Titan.API.DTOs;
using Titan.API.Services;

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
        var result = await  _projectService.CreateAsync(request);

        if (result == null) return BadRequest("Invalid Proejct data");

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await  _projectService.GetAllAsync();
        return Ok(result);
    }
}