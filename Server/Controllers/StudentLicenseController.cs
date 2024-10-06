using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Models;
using Project.Services;
using System.IO;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentLicenseController : ControllerBase
    {
        private readonly IStudentLicenseService _studentService;
        private readonly ILogger<StudentLicenseController> _logger;

        public StudentLicenseController(IStudentLicenseService studentService, ILogger<StudentLicenseController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        public async Task<IActionResult> PostApplication([FromForm] string application, [FromForm] IFormFile file)
        {
            _logger.LogInformation("Received a new application submission.");

            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded.");
                return BadRequest("No file uploaded.");
            }

            if (string.IsNullOrEmpty(application))
            {
                _logger.LogWarning("No application data provided.");
                return BadRequest("No application data provided.");
            }

            StudentLicenseApplication? applicationObj;
            try
            {
                applicationObj = JsonConvert.DeserializeObject<StudentLicenseApplication>(application);
                if (applicationObj == null)
                {
                    throw new Exception("Deserialized application object is null.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize application data.");
                return BadRequest("Invalid application data.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                return BadRequest(ModelState);
            }

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                applicationObj.Licencepicture_path = filePath;

                var result = await _studentService.AddStudentLicenseAsync(applicationObj);

                _logger.LogInformation("Application submitted successfully.");

                return Ok(new { message = "Application submitted successfully.", application = applicationObj });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the application.");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            _logger.LogInformation("Fetching all applications.");
            var applications = await _studentService.GetAllApplicationsAsync();
            _logger.LogInformation("Fetched {Count} applications.", applications.Count);
            return Ok(applications);
        }

        [HttpGet("picture/{id}")]
        public async Task<IActionResult> GetPicture(int id)
        {
            _logger.LogInformation("Fetching picture for application with ID {Id}.", id);

            try
            {
                var application = await _studentService.GetStudentLicenseByIdAsync(id);
                if (application == null || string.IsNullOrEmpty(application.Licencepicture_path))
                {
                    _logger.LogWarning("Application with ID {Id} not found or no picture available.", id);
                    return NotFound();
                }

                var filePath = application.Licencepicture_path;
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                _logger.LogInformation("Picture for application with ID {Id} fetched successfully.", id);
                return File(fileBytes, "application/octet-stream");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the picture.");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}