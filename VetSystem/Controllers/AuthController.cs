using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Logging;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using VetSystem.Models;
using VetSystem.Data;

namespace VetRecordSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(DataContext context, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            _logger.LogInformation($"Register attempt for user: {user.Username}");

            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                _logger.LogWarning($"Username already exists: {user.Username}");
                return BadRequest(new { message = "Username already exists." });
            }

            // Hash the password before saving
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User registered successfully: {user.Username}");
            return Ok(new { message = "User registered successfully." });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            _logger.LogInformation($"Login attempt for user: {loginUser.Username}");

            try
            {
                var user = await _context.Users
                                         .FirstOrDefaultAsync(u => u.Username == loginUser.Username);

                if (user == null)
                {
                    _logger.LogWarning($"Invalid login attempt for user: {loginUser.Username}");
                    return Unauthorized(new { message = "Invalid credentials." });
                }

                // Verify the password
                if (!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
                {
                    _logger.LogWarning($"Invalid password for user: {loginUser.Username}");
                    return Unauthorized(new { message = "Invalid credentials." });
                }

                // JWT Token oluşturma
                var token = GenerateJwtToken(user);

                _logger.LogInformation($"User logged in successfully: {user.Username}");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during login for user: {loginUser.Username}");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }
    }
}
