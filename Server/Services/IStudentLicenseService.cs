using Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Services
{
    public interface IStudentLicenseService
    {
        Task<bool> StudentExists(string email);  // Checks if a student with the same email exists
        Task<StudentLicenseApplication> AddStudentLicenseAsync(StudentLicenseApplication application);  // Adds a new license application
        Task<List<StudentLicenseApplication>> GetAllApplicationsAsync();  // Retrieves all license applications
        Task<StudentLicenseApplication?> GetStudentLicenseByIdAsync(int id);  // Retrieves a license application by ID
        Task<bool> UpdateStudentLicenseAsync(StudentLicenseApplication application);  // Updates an existing license application
        Task<bool> DeleteStudentLicenseAsync(int id);  // Deletes a license application by ID
    }
}