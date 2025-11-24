using Microsoft.EntityFrameworkCore;
using Titan.API.Data;
using Titan.API.DTOs;

namespace Titan.API.Services;

public class UserService : IUserService
{
    private readonly TitanDbContext _context;

    public UserService(TitanDbContext context)
    {
        _context = context;
    }
    public async Task<bool> UpdateUsernameAsync(int userId, string newUserName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return false;

        user.Username = newUserName;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<UserDto>> GetAllEmployeesAsync()
    {
        return await _context.Users
        .Where(u => u.Role == "Employee")
        .Select(u => new UserDto
        {
            Id = u.Id,
            Email = u.Email,
            UserName = u.Username,
            Role = u.Role
        })
        .ToListAsync();
    }

}