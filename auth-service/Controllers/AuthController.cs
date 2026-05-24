using Microsoft.AspNetCore.Mvc;
using auth_service.Models;
using auth_service.Services;
using auth_service.Repositories;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace auth_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly AiAnomalyService _aiAnomalyService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserRepository userRepository,
            IJwtService jwtService,
            AiAnomalyService aiAnomalyService,
            ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _aiAnomalyService = aiAnomalyService;
            _logger = logger;
        }

        // ===================== REGISTER =====================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (await _userRepository.UserExistsAsync(model.Username))
            {
                return BadRequest(new { message = "Username already exists" });
            }

            var user = new User
            {
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Email = model.Email,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateUserAsync(user);

            _logger.LogInformation($"New user registered: {model.Username}");

            return Ok(new { message = "User registered successfully" });
        }

        // ===================== LOGIN (me AI + Blocking) =====================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userRepository.GetByUsernameAsync(model.Username);

            if (user == null)
            {
                await LogFailedAttempt(model.Username, "User not found");
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Kontrolli i bllokimit
            if (await _userRepository.IsAccountBlockedAsync(user.Id))
            {
                var remaining = await _userRepository.GetBlockRemainingMinutesAsync(user.Id);
                return BadRequest(new 
                { 
                    message = $"Account is blocked. Try again in {remaining} minutes." 
                });
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            var loginEvent = new LoginEvent
            {
                UserId = user.Id,
                Username = user.Username,
                LoginTime = DateTime.UtcNow,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                UserAgent = Request.Headers["User-Agent"].ToString() ?? "unknown",
                Success = passwordValid,
                FailedAttempts = passwordValid ? 0 : await _userRepository.IncrementFailedAttemptsAsync(user.Id)
            };

            // Ruaj attempt-in
            await _userRepository.LogLoginAttemptAsync(loginEvent);

            if (!passwordValid)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Reset failed attempts
            await _userRepository.ResetFailedAttemptsAsync(user.Id);

            // ===================== AI ANOMALY DETECTION =====================
            var aiResponse = await _aiAnomalyService.AnalyzeLoginAsync(loginEvent);

            _logger.LogInformation($"AI Score for {user.Username}: {aiResponse.RiskScore} | Anomaly: {aiResponse.Anomaly}");

            // Nëse AI detekton problem → blloko për 5 minuta
            if (aiResponse.BlockAccount || aiResponse.Anomaly)
            {
                await _userRepository.BlockAccountAsync(user.Id, TimeSpan.FromMinutes(5));

                _logger.LogWarning($"🚨 ACCOUNT BLOCKED BY AI: {user.Username} | Risk: {aiResponse.RiskScore}");

                return BadRequest(new 
                { 
                    message = "Suspicious login activity detected. Account blocked for 5 minutes.",
                    riskScore = aiResponse.RiskScore,
                    riskLevel = aiResponse.RiskLevel
                });
            }

            // Login i suksesshëm
            var token = _jwtService.GenerateToken(user);

            return Ok(new 
            { 
                message = "Login successful",
                token = token,
                riskScore = aiResponse.RiskScore,
                riskLevel = aiResponse.RiskLevel
            });
        }

        private async Task LogFailedAttempt(string username, string reason)
        {
            _logger.LogWarning($"Failed login attempt for {username}. Reason: {reason}");
        }
    }
}
