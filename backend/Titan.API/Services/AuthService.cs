using Microsoft.EntityFrameworkCore;
using Titan.API.DTOs;
using Titan.API.Data;
using Titan.API.Models;
using BCrypt.Net;

namespace Titan.API.Services;

public class AuthService : IAuthService
{
    private readonly TitanDbContext _context;
    public AuthService(TitanDbContext context)
    {
        _context = context;
    }

    public async Task<User?> RegisterAsync(RegisterRequestDto request)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == request.Email);

        if (exists) return null;

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = request.Role,
            Username = null,
            CreatedOn = DateTime.UtcNow
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }

    public Task<string?> LoginAsync(LoginRequestDto request)
    {
        throw new NotImplementedException();
    }
}