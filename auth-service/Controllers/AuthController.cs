using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;           
using auth_service.Data;
using auth_service.Models;
using auth_service.DTOs;
using auth_service.Services;
using BCrypt.Net;

namespace auth_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("Username already exists");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, 12),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            // Kontroll nëse është i bllokuar
            if (user.IsBlocked && user.BlockedUntil > DateTime.UtcNow)
                return BadRequest($"Account is blocked until {user.BlockedUntil}");

            var token = _jwtService.GenerateToken(user);

            return Ok(new 
            { 
                token = token,
                userId = user.Id,
                username = user.Username,
                expiresIn = 30
            });
        }
    }
}
