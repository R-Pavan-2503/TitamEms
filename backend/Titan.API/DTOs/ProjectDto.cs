namespace Titan.API.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedOn { get; set; }
}