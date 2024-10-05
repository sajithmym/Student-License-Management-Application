using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentLicenseController : ControllerBase
    {
        private readonly IStudentLicenseService _studentService;

        public StudentLicenseController(IStudentLicenseService studentService)
        {
            _studentService = studentService;
        }

        // POST: api/studentlicense
        [HttpPost]
        [EnableCors("AllowAllOrigins")]
        public async Task<IActionResult> PostApplication([FromForm] StudentLicenseApplication application, IFormFile file)
        {
            
            // Check if a file was uploaded
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Validate the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            

            try
            {
                // Define the uploads folder path
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                // Create the uploads folder if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique file name and save the file
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Assign the file path to the application model
                application.Licencepicture_path = filePath;

                // Save the application to the database
                var result = await _studentService.AddStudentLicenseAsync(application);

                // Return a success response
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Return a bad request with the exception message
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }



        // GET: api/studentlicense
        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            var applications = await _studentService.GetAllApplicationsAsync();
            return Ok(applications);
        }
    }
}
