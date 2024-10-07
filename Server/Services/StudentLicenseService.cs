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

        public async Task<StudentLicenseApplication?> GetStudentLicenseByIdAsync(int id)
        {
            return await _context.StudentLicenseApplications.FindAsync(id);
        }

        public async Task<bool> UpdateStudentLicenseAsync(StudentLicenseApplication application)
        {
            var existingApplication = await _context.StudentLicenseApplications.FindAsync(application.Id);
            if (existingApplication == null)
            {
                return false;
            }

            existingApplication.CourseTitle = application.CourseTitle;
            existingApplication.LicenceStatus = application.LicenceStatus;
            existingApplication.ApprovalStatus = application.ApprovalStatus;
            existingApplication.LicenceExpiryDate = application.LicenceExpiryDate;
            // Update other fields as necessary

            _context.StudentLicenseApplications.Update(existingApplication);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentLicenseAsync(int id)
        {
            var application = await _context.StudentLicenseApplications.FindAsync(id);
            if (application == null)
            {
                return false;
            }

            _context.StudentLicenseApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}