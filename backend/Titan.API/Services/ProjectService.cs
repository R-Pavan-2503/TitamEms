using Microsoft.AspNetCore.Mvc;
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

    public async Task<bool> AssignEmployeeAsync(AssignProjectDto request)
    {
        var projectExists = await _context.Projects.AnyAsync(p => p.Id == request.ProjectId);

        if (!projectExists) return false;

        var employeeExists = await _context.Users.AnyAsync(u => u.Id == request.EmployeeId);

        if (!employeeExists) return false;

        var assignment = new ProjectEmployee
        {
            ProjectId = request.ProjectId,
            EmployeeId = request.EmployeeId,
            AssignedOn = DateTime.UtcNow
        };

        try
        {
            _context.ProjectEmployees.Add(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }


    }

    public async Task<bool> RemoveEmployeeAsync(AssignProjectDto request)
    {
        var assignment = await _context.ProjectEmployees
    .FirstOrDefaultAsync(pe => pe.ProjectId == request.ProjectId && pe.EmployeeId == request.EmployeeId);

        if (assignment == null) return false;

        _context.ProjectEmployees.Remove(assignment);
        await _context.SaveChangesAsync();

        return true;

    }

    public async Task<IEnumerable<ProjectDto>> GetProjectsForUserAsync(int userId)
    {
        return await _context.ProjectEmployees
        .Where(pe => pe.EmployeeId == userId)
        .Select(pe => pe.Project)
        .Where(p => p != null)
        .Select(p => new ProjectDto
        {
            Id = p!.Id,
            Name = p.Name,
            Description = p.Description,
            CreatedOn = p.CreatedOn
        })
        .ToListAsync();
    }
}