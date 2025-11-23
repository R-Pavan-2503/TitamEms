namespace Titan.API.DTOs;

public class RegisterRequestDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!; // raw password (will be hashed later)
    public string Role { get; set; } = default!;
}
