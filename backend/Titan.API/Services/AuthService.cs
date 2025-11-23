using Microsoft.EntityFrameworkCore;
using Titan.API.DTOs;
using Titan.API.Data;
using Titan.API.Models;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Titan.API.Services;

public class AuthService : IAuthService
{
    private readonly TitanDbContext _context;
    private readonly IConfiguration _config;
    public AuthService(TitanDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
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

    public async Task<string?> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null) return null;

        bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);


        if (!passwordValid) return null;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = _config["JwtSettings:Key"];
        var issuer = _config["JwtSettings:Issuer"];
        var audience = _config["JwtSettings:Audience"];

        if (string.IsNullOrEmpty(key))
            throw new Exception("JWT Key is missing in configuration.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}