using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Services
{
    public class StudentLicenseService : IStudentLicenseService
    {
        private readonly AppDbContext _context;

        public StudentLicenseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> StudentExists(string email)
        {
            return await _context.StudentLicenseApplications.AnyAsync(s => s.Email == email);
        }

        public async Task<StudentLicenseApplication> AddStudentLicenseAsync(StudentLicenseApplication application)
        {
            if (await StudentExists(application.Email))
            {
                throw new System.Exception("A student with the same email already exists.");
            }

            _context.StudentLicenseApplications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<List<StudentLicenseApplication>> GetAllApplicationsAsync()
        {
            return await _context.StudentLicenseApplications.ToListAsync();
        }
    }
}
