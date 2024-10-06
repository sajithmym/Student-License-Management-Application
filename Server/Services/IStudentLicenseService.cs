using Project.Models;

namespace Project.Services
{
    public interface IStudentLicenseService
    {
        Task<bool> StudentExists(string email);  // Checks if a student with the same email exists
        Task<StudentLicenseApplication> AddStudentLicenseAsync(StudentLicenseApplication application);  // Adds a new license application
        Task<List<StudentLicenseApplication>> GetAllApplicationsAsync();  // Retrieves all license applications
        Task<StudentLicenseApplication?> GetStudentLicenseByIdAsync(int id);  // Retrieves a license application by ID
    }
}
