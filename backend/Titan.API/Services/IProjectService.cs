using Titan.API.DTOs;

namespace Titan.API.Services;

public interface IProjectService
{
    public Task<ProjectDto?> CreateAsync(CreateProjectDto request);

    public Task<IEnumerable<ProjectDto>> GetAllAsync();

    public Task<bool> AssignEmployeeAsync(AssignProjectDto request);
}