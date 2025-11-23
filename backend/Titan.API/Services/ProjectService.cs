using Microsoft.EntityFrameworkCore;
using Titan.API.Data;
using Titan.API.DTOs;
using Titan.API.Models;

namespace Titan.API.Services;


public class ProjectService : IProjectService
{
    private readonly TitanDbContext _context;

    public ProjectService(TitanDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectDto?> CreateAsync(CreateProjectDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return null;

        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            CreatedOn = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedOn = project.CreatedOn
        };
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        return await _context.Projects
        .Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            CreatedOn = p.CreatedOn
        })
        .ToListAsync();
    }
}