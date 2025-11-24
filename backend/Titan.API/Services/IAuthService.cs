namespace Titan.API.Services;

using Titan.API.Models;
using Titan.API.DTOs;
public interface IAuthService
{
    Task<User?> RegisterAsync(RegisterRequestDto request);

    Task<string?> LoginAsync(LoginRequestDto request);
}