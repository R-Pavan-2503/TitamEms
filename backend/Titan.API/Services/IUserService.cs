using Titan.API.DTOs;
namespace Titan.API.Services;

public interface IUserService
{
    public Task<bool> UpdateUsernameAsync(int userId, string newUserName);

    public Task<IEnumerable<UserDto>> GetAllEmployeesAsync();
}