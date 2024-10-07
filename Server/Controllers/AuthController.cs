using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login attempt.");

            // Get the password from the .env file
            var envPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
            if (string.IsNullOrEmpty(envPassword))
            {
                throw new InvalidOperationException("ADMIN_PASSWORD environment variable is not set.");
            }

            // Validate the user credentials (only password)
            if (request.Password == envPassword)
            {
                var token = GenerateJwtToken();
                _logger.LogInformation("Login successful.");

                // Set the token as a cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(GetJwtExpiryHours())
                };
                Response.Cookies.Append("jwt", token, cookieOptions);

                return Ok(new { token });
            }

            _logger.LogWarning("Login failed.");
            return Unauthorized(new { message = "Invalid credentials" });
        }

        [HttpGet("validate-token")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            return Ok(new { valid = true });
        }

        private string GenerateJwtToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT_SECRET_KEY environment variable is not set.");
            }
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "admin") }),
                Expires = DateTime.UtcNow.AddHours(GetJwtExpiryHours()),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private int GetJwtExpiryHours()
        {
            var expiryHoursString = Environment.GetEnvironmentVariable("JWT_EXPIRY_HOURS");
            if (int.TryParse(expiryHoursString, out int expiryHours))
            {
                return expiryHours;
            }
            throw new InvalidOperationException("JWT_EXPIRY_HOURS environment variable is not set or is not a valid integer.");
        }
    }

    public class LoginRequest
    {
        public string? Password { get; set; }
    }
}