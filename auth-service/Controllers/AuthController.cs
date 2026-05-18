using Microsoft.AspNetCore.Mvc;
using auth_service.Data;
using auth_service.Models;
using auth_service.DTOs;
using BCrypt.Net;

namespace auth_service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // check if user exists
        var exists = _context.Users.Any(u => u.Email == dto.Email);

        if (exists)
            return BadRequest("User already exists");

        // hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully");
    }
}
