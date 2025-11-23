namespace Titan.API.DTOs;

public class CreateProjectDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}