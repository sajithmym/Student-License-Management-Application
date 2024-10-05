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
        public async Task<IActionResult> PostApplication([FromBody] StudentLicenseApplication application)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _studentService.AddStudentLicenseAsync(application);
                    return Ok(result);
                }
                catch (System.Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Invalid data.");
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
