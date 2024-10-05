using Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Services
{
    public interface IStudentLicenseService
    {
        Task<bool> StudentExists(string email);
        Task<StudentLicenseApplication> AddStudentLicenseAsync(StudentLicenseApplication application);
        Task<List<StudentLicenseApplication>> GetAllApplicationsAsync();
    }
}
