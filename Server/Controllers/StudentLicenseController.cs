using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Models;
using Project.Services;

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

        /// <summary>
        /// Handles the submission of a new student license application.
        /// </summary>
        /// <param name="application">The application data as a JSON string.</param>
        /// <param name="file">The uploaded file.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
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

        /// <summary>
        /// Retrieves all student license applications.
        /// </summary>
        /// <returns>A list of student license applications.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StudentLicenseApplication>>> GetApplications()
        {
            _logger.LogInformation("Fetching all applications.");
            var applications = await _studentService.GetAllApplicationsAsync();
            return Ok(applications);
        }

        /// <summary>
        /// Retrieves the picture associated with a specific application.
        /// </summary>
        /// <param name="id">The ID of the application.</param>
        /// <returns>The picture file.</returns>
        [HttpGet("picture/{id}")]
        [Authorize]
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
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var fileExtension = Path.GetExtension(filePath);

                _logger.LogInformation("Picture for application with ID {Id} fetched successfully.", id);

                var fullFileName = $"{fileName}{fileExtension}";

                Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{fullFileName}\"");
                return File(fileBytes, "application/octet-stream");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the picture.");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing student license application.
        /// </summary>
        /// <param name="id">The ID of the application to update.</param>
        /// <param name="application">The updated application data.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditApplication(int id, [FromBody] StudentLicenseApplication application)
        {
            _logger.LogInformation("Editing application with ID {Id}.", id);

            if (id != application.Id)
            {
                return BadRequest("ID mismatch");
            }
            if (!ModelState.IsValid || application == null)
            {
                return BadRequest("Invalid application data.");
            }

            try
            {
                var result = await _studentService.UpdateStudentLicenseAsync(application);
                if (result)
                {
                    _logger.LogInformation("Application with ID {Id} updated successfully.", id);
                    return Ok(new { message = "Application updated successfully." });
                }
                else
                {
                    _logger.LogWarning("Application with ID {Id} not found.", id);
                    return NotFound(new { message = "Application not found." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the application.");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a specific student license application.
        /// </summary>
        /// <param name="id">The ID of the application to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            _logger.LogInformation("Deleting application with ID {Id}.", id);

            try
            {
                var result = await _studentService.DeleteStudentLicenseAsync(id);
                if (result)
                {
                    _logger.LogInformation("Application with ID {Id} deleted successfully.", id);
                    return Ok(new { message = "Application deleted successfully." });
                }
                else
                {
                    _logger.LogWarning("Application with ID {Id} not found.", id);
                    return NotFound(new { message = "Application not found." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the application.");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}