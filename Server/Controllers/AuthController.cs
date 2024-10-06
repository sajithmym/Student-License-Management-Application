using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("verify-password")]
        [EnableCors("AllowAllOrigins")]
        public IActionResult VerifyPassword([FromBody] PasswordRequest request)
        {
            _logger.LogInformation("Password verification attempt.");

            if (request.Password == "123")
            {
                _logger.LogInformation("Password verification successful.");
                return Ok(new { success = true });
            }

            _logger.LogWarning("Password verification failed.");
            return Unauthorized(new { success = false });
        }
    }

    public class PasswordRequest
    {
        public string Password { get; set; } = string.Empty;
    }
}